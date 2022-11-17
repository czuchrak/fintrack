using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthPart;

public class RemoveNetWorthPartCommandHandler : IRequestHandler<RemoveNetWorthPartCommand, Unit>
{
    private readonly DatabaseContext _context;

    public RemoveNetWorthPartCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveNetWorthPartCommand request, CancellationToken cancellationToken)
    {
        var partId = request.PartId;
        var userId = request.UserId;

        var parts = await _context.NetWorthParts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var partToRemove = parts.Single(x => x.Id == partId);
        var partsToUpdate = parts.Where(x => x.Order > partToRemove.Order).ToList();

        partsToUpdate.ForEach(x => x.Order--);

        _context.Remove(partToRemove);
        _context.UpdateRange(partsToUpdate);

        if (parts.Count == 1)
        {
            var entriesToRemove = _context.NetWorthEntries
                .Where(x => x.UserId == userId);
            _context.NetWorthEntries.RemoveRange(entriesToRemove);
        }

        var goalsToRemove = await _context.NetWorthGoals
            .Where(x => x.UserId == userId)
            .Include(x => x.GoalParts)
            .Where(x => x.GoalParts.Count == 1 && x.GoalParts.First().NetWorthPartId == partId)
            .ToListAsync(cancellationToken);

        if (goalsToRemove.Any())
            _context.NetWorthGoals.RemoveRange(goalsToRemove);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}