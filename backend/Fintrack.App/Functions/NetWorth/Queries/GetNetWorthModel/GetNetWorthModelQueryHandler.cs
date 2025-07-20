using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.App.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Queries.GetNetWorthModel;

public class GetNetWorthModelQueryHandler : IRequestHandler<GetNetWorthModelQuery, NetWorthModel>
{
    private readonly DatabaseContext _context;

    public GetNetWorthModelQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<NetWorthModel> Handle(GetNetWorthModelQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId!;

        var parts = await GetParts(userId, cancellationToken);
        var entries = await GetEntries(userId, cancellationToken);
        var goals = await GetGoals(userId, cancellationToken);
        var rates = await GetExchangeRates(parts, entries);

        return new NetWorthModel
        {
            Entries = entries,
            Goals = goals,
            Parts = parts,
            Rates = rates
        };
    }

    private async Task<IList<NetWorthPartModel>> GetParts(string userId, CancellationToken cancellationToken)
    {
        return await _context.NetWorthParts
            .Where(x => x.UserId == userId)
            .Select(x => new NetWorthPartModel
            {
                Id = x.Id,
                Name = x.Name,
                IsVisible = x.IsVisible,
                Order = x.Order,
                Type = x.Type,
                Currency = x.Currency
            })
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);
    }

    private async Task<IList<NetWorthGoalModel>> GetGoals(string userId, CancellationToken cancellationToken)
    {
        return await _context.NetWorthGoals
            .Include(x => x.GoalParts)
            .Where(x => x.UserId == userId)
            .Select(x => new NetWorthGoalModel
            {
                Id = x.Id,
                Name = x.Name,
                Deadline = x.Deadline,
                Parts = x.GoalParts.Select(y => y.NetWorthPartId),
                Value = x.Value,
                ReturnRate = x.ReturnRate
            })
            .OrderBy(x => x.Deadline)
            .ToListAsync(cancellationToken);
    }

    private async Task<IList<NetWorthEntryModel>> GetEntries(string userId, CancellationToken cancellationToken)
    {
        return (await _context.NetWorthEntries
                .Where(x => x.UserId == userId)
                .Include(x => x.EntryParts)
                .Select(x => new
                {
                    x.Id,
                    Date = x.Date.ToString("yyyy-MM-dd"),
                    x.ExchangeRateDate,
                    PartValues = x.EntryParts.Select(y => new { y.NetWorthPartId, y.Value })
                })
                .ToListAsync(cancellationToken))
            .Select(x => new NetWorthEntryModel
            {
                Id = x.Id,
                Date = x.Date,
                ExchangeRateDate = x.ExchangeRateDate,
                PartValues = x.PartValues.ToDictionary(y => y.NetWorthPartId.ToString(), y => y.Value)
            })
            .ToList();
    }

    private async Task<IList<ExchangeRateModel>> GetExchangeRates(IEnumerable<NetWorthPartModel> parts,
        IEnumerable<NetWorthEntryModel> entries)
    {
        var currencies = parts.Select(x => x.Currency).Distinct().ToList();
        var exchangeDates = entries.Select(x => x.ExchangeRateDate).Distinct().ToList();

        return await _context.ExchangeRates
            .Where(x => exchangeDates.Contains(x.Date) && currencies.Contains(x.Currency))
            .Select(x => new ExchangeRateModel
            {
                Date = x.Date,
                Currency = x.Currency,
                Rate = x.Rate
            })
            .ToListAsync();
    }
}