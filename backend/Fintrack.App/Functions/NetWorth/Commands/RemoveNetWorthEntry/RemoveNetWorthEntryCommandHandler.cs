using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthEntry;

public class RemoveNetWorthEntryCommandHandler : IRequestHandler<RemoveNetWorthEntryCommand, Unit>
{
    private readonly DatabaseContext _context;

    public RemoveNetWorthEntryCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveNetWorthEntryCommand request, CancellationToken cancellationToken)
    {
        var entryId = request.EntryId;
        var userId = request.UserId;

        var entry = await _context.NetWorthEntries
            .SingleAsync(x => x.Id == entryId && x.UserId == userId, cancellationToken);

        _context.Remove(entry);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}