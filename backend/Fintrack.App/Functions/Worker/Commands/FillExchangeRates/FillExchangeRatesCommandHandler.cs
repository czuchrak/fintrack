using Fintrack.App.HttpClients;
using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Worker.Commands.FillExchangeRates;

public class FillExchangeRatesCommandHandler : IRequestHandler<FillExchangeRatesCommand, Unit>
{
    private const int LastDays = 3;
    private readonly DatabaseContext _context;
    private readonly ILogger<FillExchangeRatesCommandHandler> _logger;
    private readonly INbpHttpClient _nbpHttpClient;

    public FillExchangeRatesCommandHandler(DatabaseContext context,
        INbpHttpClient nbpHttpClient,
        ILogger<FillExchangeRatesCommandHandler> logger)
    {
        _context = context;
        _nbpHttpClient = nbpHttpClient;
        _logger = logger;
    }

    public async Task<Unit> Handle(FillExchangeRatesCommand request, CancellationToken cancellationToken)
    {
        if (request.FillAll) await FillRates(DateTime.Parse("2020-01-01"), DateTime.Now, cancellationToken);

        var lastRate = await _context.ExchangeRates
            .OrderByDescending(x => x.Date)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastRate == null || DateTime.Now.Date != lastRate.Date)
            await FillRates(DateTime.Now.AddDays(-LastDays).Date, DateTime.Now, cancellationToken);

        return Unit.Value;
    }

    private async Task FillRates(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        const int maxDays = 90;
        var currentFrom = from;

        while (currentFrom <= to)
        {
            var currentTo = currentFrom.AddDays(maxDays - 1);
            if (currentTo > to) currentTo = to;

            var tables = await _nbpHttpClient.GetRates(currentFrom, currentTo);

            var currencies = await _context.Currencies
                .Where(x => x.Code != "PLN")
                .ToListAsync(cancellationToken);

            var dbRates = await _context.ExchangeRates
                .Where(x => x.Date >= currentFrom && x.Date <= currentTo)
                .ToListAsync(cancellationToken);

            foreach (var table in tables)
            foreach (var currency in currencies
                         .Where(currency =>
                             !dbRates.Any(x =>
                                 x.Date.Date == table.EffectiveDate.Date &&
                                 x.Currency == currency.Code)))
                _context.ExchangeRates.Add(new ExchangeRate
                {
                    Date = table.EffectiveDate,
                    Currency = currency.Code,
                    Rate = table.Rates.First(x => x.Code == currency.Code).Mid
                });

            if (await _context.SaveChangesAsync(cancellationToken) > 0)
                _logger.LogInformation("Rates from {CurrentFrom} to {CurrentTo} have been refreshed", currentFrom,
                    currentTo);

            currentFrom = currentTo.AddDays(1);
        }
    }
}