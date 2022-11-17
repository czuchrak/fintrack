using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Worker.Commands.RemoveUnnecessaryRates;

public class RemoveUnnecessaryRatesCommandHandler : IRequestHandler<RemoveUnnecessaryRatesCommand, Unit>
{
    private const int LastDays = 5;
    private readonly DatabaseContext _context;
    private readonly ILogger<RemoveUnnecessaryRatesCommandHandler> _logger;

    public RemoveUnnecessaryRatesCommandHandler(DatabaseContext context,
        ILogger<RemoveUnnecessaryRatesCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(RemoveUnnecessaryRatesCommand request, CancellationToken cancellationToken)
    {
        var rates = await _context.ExchangeRates
            .Where(x => x.Date < DateTime.Now.AddDays(-LastDays))
            .ToListAsync(cancellationToken);

        var usingDates = await GetUsingDates(cancellationToken);

        var ratesToRemove = rates.Where(x => !usingDates.Contains(x.Date)).ToList();

        if (ratesToRemove.Any())
        {
            _context.ExchangeRates.RemoveRange(ratesToRemove);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"{ratesToRemove.Count} unnecessary rates have been removed");
        }

        return Unit.Value;
    }

    private async Task<IEnumerable<DateTime>> GetUsingDates(CancellationToken cancellationToken)
    {
        var usingDates = await _context.NetWorthEntries
            .Select(x => x.ExchangeRateDate)
            .ToListAsync(cancellationToken);

        var date = DateTime.Parse("2020-01-01");
        var endDate = DateTime.Now;

        while (date < endDate)
        {
            var exchangeRateDate = await GetExchangeDate(date);
            usingDates.Add(exchangeRateDate);
            date = date.AddMonths(1);
        }

        usingDates.Add(await GetExchangeDate(DateTime.Now));

        return usingDates.Distinct();
    }

    private async Task<DateTime> GetExchangeDate(DateTime date)
    {
        return await _context.ExchangeRates.Where(x => x.Date <= date)
            .Select(x => x.Date)
            .OrderByDescending(x => x.Date)
            .FirstAsync();
    }
}