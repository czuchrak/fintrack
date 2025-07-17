using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthGoal;

public class RemoveNetWorthGoalCommand : RequestBase, IRequest<Unit>
{
    public Guid GoalId { get; set; }
}