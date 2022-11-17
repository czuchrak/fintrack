using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthEntry;

public class AddNetWorthEntryCommandHandler : IRequestHandler<AddNetWorthEntryCommand, Unit>
{
    private readonly DatabaseContext _context;

    public AddNetWorthEntryCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddNetWorthEntryCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;
        var date = DateTime.Parse(model.Date).Date;

        _context.NetWorthEntries.Add(new NetWorthEntry
        {
            UserId = userId,
            Date = date,
            ExchangeRateDate = await GetExchangeDate(date, cancellationToken),
            EntryParts = model.PartValues
                .Where(x => x.Value != 0)
                .Select(x => new NetWorthEntryPart
                {
                    NetWorthPartId = Guid.Parse(x.Key),
                    Value = x.Value
                })
                .ToList()
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<DateTime> GetExchangeDate(DateTime date, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var lastDate = date.Month == now.Month && date.Year == now.Year ? now : date;

        return await _context.ExchangeRates.Where(x => x.Date <= lastDate)
            .Select(x => x.Date)
            .OrderByDescending(x => x.Date)
            .FirstAsync(cancellationToken);
    }
}