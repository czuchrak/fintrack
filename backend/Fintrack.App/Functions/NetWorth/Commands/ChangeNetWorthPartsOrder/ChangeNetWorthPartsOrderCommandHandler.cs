using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;

public class ChangeNetWorthPartsOrderCommandHandler(DatabaseContext context)
    : IRequestHandler<ChangeNetWorthPartsOrderCommand, Unit>
{
    public async Task<Unit> Handle(ChangeNetWorthPartsOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var parts = await context.NetWorthParts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var order = 1;

        foreach (var id in request.PartIds)
        {
            var part = parts.Single(x => x.Id == id);
            part.Order = order++;
        }

        context.UpdateRange(parts);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}