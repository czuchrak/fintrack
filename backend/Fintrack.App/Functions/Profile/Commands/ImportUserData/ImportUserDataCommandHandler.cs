using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Profile.Commands.ImportUserData;

public class ImportUserDataCommandHandler(DatabaseContext context, ILogger<ImportUserDataCommandHandler> logger)
    : IRequestHandler<ImportUserDataCommand, ImportUserDataResult>
{
    public async Task<ImportUserDataResult> Handle(ImportUserDataCommand request, CancellationToken cancellationToken)
    {
        var result = new ImportUserDataResult();

        try
        {
            var userId = request.UserId;

            await ClearUserData(userId, cancellationToken);

            using var memoryStream = new MemoryStream(request.FileContent);
            using var reader = new StreamReader(memoryStream, Encoding.UTF8);

            string currentSection = null;
            while (await reader.ReadLineAsync(cancellationToken) is { } line)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentSection = null;
                    continue;
                }

                if (line.StartsWith("## ")) currentSection = line.Substring(3).Trim();

                if (currentSection == "Net Worth")
                {
                    await ProcessNetWorthData(reader, userId, result, cancellationToken);
                    currentSection = null;
                }
                else if (currentSection == "Properties")
                {
                    await ProcessPropertyData(reader, userId, cancellationToken, result);
                    currentSection = null;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Błąd podczas importu danych użytkownika {UserId}", request.UserId);
            result.Errors.Add("Błąd podczas importu danych.");
        }

        return result;
    }

    private async Task ClearUserData(string userId, CancellationToken cancellationToken)
    {
        var netWorthEntries = await context.NetWorthEntries
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        context.NetWorthEntries.RemoveRange(netWorthEntries);

        var netWorthParts = await context.NetWorthParts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        context.NetWorthParts.RemoveRange(netWorthParts);

        var properties = await context.Properties
            .Where(x => x.UserId == userId)
            .Include(p => p.Transactions)
            .ToListAsync(cancellationToken);

        foreach (var property in properties) context.PropertyTransactions.RemoveRange(property.Transactions);
        context.Properties.RemoveRange(properties);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessNetWorthData(StreamReader reader, string userId,
        ImportUserDataResult result, CancellationToken cancellationToken)
    {
        var headerLine = await reader.ReadLineAsync(cancellationToken);
        if (string.IsNullOrEmpty(headerLine))
        {
            result.Errors.Add("Brak składników");
            return;
        }

        var headers = headerLine.Split(',');
        var newParts = new List<NetWorthPart>();

        var validCurrencies = await context.Currencies
            .Select(c => c.Code)
            .ToListAsync(cancellationToken);

        for (var i = 1; i < headers.Length; i++)
        {
            var header = headers[i];
            var match = Regex.Match(header, @"(.*) \((.*)\|(.*)\)");
            if (!match.Success)
            {
                result.Errors.Add($"Nieprawidłowy format składnika: {header}. Oczekiwano 'Nazwa (Waluta|Typ)'.");
                return;
            }

            if (!new[] { "asset", "liability" }.Contains(match.Groups[3].Value.Trim().ToLower()))
            {
                result.Errors.Add(
                    $"Nieprawidłowy typ składnika: {match.Groups[3].Value.Trim()}: {header}. Użyj 'asset' (aktywo) lub 'liability' (zobowiązanie).");
                return;
            }

            if (!validCurrencies.Contains(match.Groups[2].Value.Trim()))
            {
                result.Errors.Add(
                    $"Nieobsługiwana waluta: {match.Groups[2].Value.Trim()}. Obsługiwane waluty: {string.Join(", ", validCurrencies)}");
                return;
            }

            newParts.Add(new NetWorthPart
            {
                Id = Guid.NewGuid(),
                Name = match.Groups[1].Value.Trim(),
                Currency = match.Groups[2].Value.Trim(),
                Type = match.Groups[3].Value.Trim(),
                UserId = userId,
                Order = i,
                IsVisible = true
            });
        }

        result.PartsAdded = newParts.Count;
        await context.NetWorthParts.AddRangeAsync(newParts, cancellationToken);

        var fileEntries = new HashSet<(int Year, int Month)>();

        while (await reader.ReadLineAsync(cancellationToken) is { } line && !string.IsNullOrWhiteSpace(line))
        {
            var values = line.Split(',');
            if (values.Length != headers.Length)
            {
                result.Errors.Add($"Nieprawidłowa liczba kolumn w: {line}");
                return;
            }

            var entry = new NetWorthEntry
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = DateTime.Parse(values[0], CultureInfo.InvariantCulture),
                ExchangeRateDate = await GetExchangeDate(DateTime.Parse(values[0], CultureInfo.InvariantCulture),
                    cancellationToken),
                EntryParts = new List<NetWorthEntryPart>()
            };

            if (entry.Date < new DateTime(2020, 1, 1))
            {
                result.Errors.Add(
                    $"Nieprawidłowa data: {entry.Date:yyyy-MM-dd}. Minimalna data to 1 stycznia 2020.");
                return;
            }

            var firstDayOfNextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            if (entry.Date > firstDayOfNextMonth)
            {
                result.Errors.Add(
                    $"Nieprawidłowa data: {entry.Date:yyyy-MM-dd}. Data nie może być większa niż ({firstDayOfNextMonth:yyyy-MM-dd}).");
                return;
            }

            var yearMonth = (entry.Date.Year, entry.Date.Month);

            if (!fileEntries.Add(yearMonth))
            {
                result.Errors.Add(
                    $"Duplikat dla roku {entry.Date.Year} i miesiąca {entry.Date.Month}.");
                return;
            }

            for (var i = 1; i < values.Length; i++)
                if (decimal.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                {
                    if (value > 10_000_000)
                    {
                        result.Errors.Add(
                            $"Nieprawidłowa kwota: {value}. Wartość nie może być większa niż 10 mln.");
                        return;
                    }

                    if (value != 0)
                        entry.EntryParts.Add(new NetWorthEntryPart
                        {
                            NetWorthPartId = newParts[i - 1].Id,
                            Value = value
                        });
                }

            await context.NetWorthEntries.AddAsync(entry, cancellationToken);
            result.EntriesAdded++;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessPropertyData(StreamReader reader, string userId, CancellationToken cancellationToken,
        ImportUserDataResult result)
    {
        await reader.ReadLineAsync(cancellationToken);

        var properties = new Dictionary<string, Database.Entities.Property>();
        var categories = await context.PropertyCategories
            .ToDictionaryAsync(c => c.Type, c => c, StringComparer.OrdinalIgnoreCase, cancellationToken);

        while (await reader.ReadLineAsync(cancellationToken) is { } line && !string.IsNullOrWhiteSpace(line))
        {
            var values = line.Split(',');
            if (values.Length != 5)
            {
                result.Errors.Add($"Nieprawidłowa liczba kolumn: {line}");
                return;
            }

            var propertyName = values[0].Trim();
            var categoryType = values[1].Trim();

            if (!categories.ContainsKey(categoryType))
            {
                result.Errors.Add(
                    $"Nieznany typ kategorii: {categoryType}. Dostępne typy: {string.Join(", ", categories.Keys)}");
                return;
            }

            if (!properties.ContainsKey(propertyName))
            {
                var newProperty = new Database.Entities.Property
                {
                    Id = Guid.NewGuid(),
                    Name = propertyName,
                    UserId = userId,
                    IsActive = true
                };
                await context.Properties.AddAsync(newProperty, cancellationToken);
                properties[propertyName] = newProperty;
                result.PropertiesAdded++;
            }

            var transaction = new PropertyTransaction
            {
                PropertyId = properties[propertyName].Id,
                CategoryId = categories[categoryType].Id,
                Value = decimal.Parse(values[2], CultureInfo.InvariantCulture),
                Date = DateTime.Parse(values[3], CultureInfo.InvariantCulture),
                Details = values[4]
            };

            if (transaction.Date < new DateTime(2020, 1, 1))
            {
                result.Errors.Add(
                    $"Nieprawidłowa data: {transaction.Date:yyyy-MM-dd}. Minimalna data to 1 stycznia 2020.");
                return;
            }

            var firstDayOfNextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            if (transaction.Date > firstDayOfNextMonth)
            {
                result.Errors.Add(
                    $"Nieprawidłowa data: {transaction.Date:yyyy-MM-dd}. Data nie może być większa niż ({firstDayOfNextMonth:yyyy-MM-dd}).");
                return;
            }

            await context.PropertyTransactions.AddAsync(transaction, cancellationToken);
            result.PropertyTransactionsAdded++;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<DateTime> GetExchangeDate(DateTime date, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var lastDate = date.Month == now.Month && date.Year == now.Year ? now : date;

        return await context.ExchangeRates.Where(x => x.Date <= lastDate)
            .Select(x => x.Date)
            .OrderByDescending(x => x.Date)
            .FirstAsync(cancellationToken);
    }
}