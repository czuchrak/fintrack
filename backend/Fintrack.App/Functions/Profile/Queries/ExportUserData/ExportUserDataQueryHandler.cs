using System.Globalization;
using System.Text;
using Fintrack.App.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Profile.Queries.ExportUserData;

public class ExportUserDataQueryHandler(DatabaseContext context)
    : IRequestHandler<ExportUserDataQuery, FileModel>
{
    public async Task<FileModel> Handle(ExportUserDataQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var sb = new StringBuilder();

        await AppendNetWorthData(sb, userId, cancellationToken);
        sb.AppendLine();
        await AppendPropertyData(sb, userId, cancellationToken);

        return new FileModel
        {
            Content = Encoding.UTF8.GetBytes(sb.ToString()),
            ContentType = "text/csv",
            FileName = $"fintrack_export_{DateTime.UtcNow:yyyyMMdd}.csv"
        };
    }

    private async Task AppendNetWorthData(StringBuilder sb, string userId, CancellationToken cancellationToken)
    {
        sb.AppendLine("## Net Worth");

        var parts = await context.NetWorthParts
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Order)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (parts.Count == 0) return;

        var headers = new List<string> { "Date" };
        headers.AddRange(parts.Select(p => $"{Escape(p.Name)} ({p.Currency}|{p.Type})"));
        sb.AppendLine(string.Join(",", headers));

        var entries = await context.NetWorthEntries
            .Include(e => e.EntryParts)
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.Date)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (entries.Count == 0) return;

        var partIdToIndex = parts.Select((p, i) => new { p.Id, Index = i + 1 })
            .ToDictionary(p => p.Id, p => p.Index);

        foreach (var entry in entries)
        {
            var row = new string[headers.Count];
            row[0] = entry.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            for (var i = 1; i < row.Length; i++) row[i] = "0";

            foreach (var entryPart in entry.EntryParts)
                if (partIdToIndex.TryGetValue(entryPart.NetWorthPartId, out var index))
                    row[index] = entryPart.Value.ToString(CultureInfo.InvariantCulture);

            sb.AppendLine(string.Join(",", row));
        }
    }

    private async Task AppendPropertyData(StringBuilder sb, string userId, CancellationToken cancellationToken)
    {
        sb.AppendLine("## Properties");

        var categories = await context.PropertyCategories
            .ToDictionaryAsync(c => c.Id, c => c, cancellationToken);

        var transactions = await context.PropertyTransactions
            .Include(t => t.Property)
            .Where(t => t.Property.UserId == userId)
            .OrderBy(t => t.Date)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (transactions.Count == 0) return;

        sb.AppendLine("Property Name,Category Type,Value,Date,Description");
        foreach (var t in transactions)
        {
            var row = new[]
            {
                Escape(t.Property.Name),
                Escape(categories[t.CategoryId].Type),
                t.Value.ToString(CultureInfo.InvariantCulture),
                t.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Escape(t.Details)
            };
            sb.AppendLine(string.Join(",", row));
        }
    }

    private static string Escape(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        if (!s.Contains(',') && !s.Contains('"') && !s.Contains('\n')) return s;
        return $"\"{s.Replace("\"", "\"\"")}\"";
    }
}