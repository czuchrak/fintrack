using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;

public class ChangeNetWorthPartsOrderCommandHandler : IRequestHandler<ChangeNetWorthPartsOrderCommand, Unit>
{
    private readonly DatabaseContext _context;

    public ChangeNetWorthPartsOrderCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeNetWorthPartsOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var parts = await _context.NetWorthParts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var order = 1;

        foreach (var id in request.PartIds)
        {
            var part = parts.Single(x => x.Id == id);
            part.Order = order++;
        }

        _context.UpdateRange(parts);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}