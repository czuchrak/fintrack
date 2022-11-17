using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthEntry;

public class UpdateNetWorthEntryCommandHandler : IRequestHandler<UpdateNetWorthEntryCommand, Unit>
{
    private readonly DatabaseContext _context;

    public UpdateNetWorthEntryCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateNetWorthEntryCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        var entry = await _context.NetWorthEntries
            .Include(x => x.EntryParts)
            .SingleAsync(x => x.Id == model.Id && x.UserId == userId, cancellationToken);

        entry.EntryParts = model.PartValues
            .Where(x => x.Value != 0)
            .Select(x => new NetWorthEntryPart
            {
                NetWorthPartId = Guid.Parse(x.Key),
                Value = x.Value
            })
            .ToList();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}