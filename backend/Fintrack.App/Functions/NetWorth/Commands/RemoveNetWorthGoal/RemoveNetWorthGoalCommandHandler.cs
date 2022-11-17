using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthGoal;

public class RemoveNetWorthGoalCommandHandler : IRequestHandler<RemoveNetWorthGoalCommand, Unit>
{
    private readonly DatabaseContext _context;

    public RemoveNetWorthGoalCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveNetWorthGoalCommand request, CancellationToken cancellationToken)
    {
        var goalId = request.GoalId;
        var userId = request.UserId;

        var goal = await _context.NetWorthGoals
            .SingleAsync(x => x.Id == goalId && x.UserId == userId, cancellationToken);

        _context.Remove(goal);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}