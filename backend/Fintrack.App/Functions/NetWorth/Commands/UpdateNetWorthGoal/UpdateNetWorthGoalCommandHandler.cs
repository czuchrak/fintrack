using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthGoal;

public class UpdateNetWorthGoalCommandHandler : IRequestHandler<UpdateNetWorthGoalCommand, Unit>
{
    private readonly DatabaseContext _context;

    public UpdateNetWorthGoalCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateNetWorthGoalCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        var goal = await _context.NetWorthGoals
            .Include(x => x.GoalParts)
            .SingleAsync(x => x.Id == model.Id && x.UserId == userId, cancellationToken);

        goal.Name = model.Name;
        goal.Deadline = model.Deadline.Date;
        goal.Value = model.Value;
        goal.ReturnRate = model.ReturnRate;
        goal.GoalParts = model.Parts
            .Select(x => new NetWorthGoalPart
            {
                NetWorthPartId = x
            }).ToList();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}