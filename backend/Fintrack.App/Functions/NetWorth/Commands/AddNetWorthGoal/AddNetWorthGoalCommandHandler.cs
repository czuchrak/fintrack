using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthGoal;

public class AddNetWorthGoalCommandHandler(DatabaseContext context) : IRequestHandler<AddNetWorthGoalCommand, Unit>
{
    public async Task<Unit> Handle(AddNetWorthGoalCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        context.Add(new NetWorthGoal
        {
            UserId = userId,
            Name = model.Name,
            Value = model.Value,
            ReturnRate = model.ReturnRate,
            Deadline = model.Deadline.Date,
            GoalParts = model.Parts
                .Select(x => new NetWorthGoalPart
                {
                    NetWorthPartId = x
                }).ToList()
        });

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}