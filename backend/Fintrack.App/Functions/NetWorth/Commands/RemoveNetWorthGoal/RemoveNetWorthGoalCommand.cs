using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthGoal;

public class RemoveNetWorthGoalCommand : RequestBase, IRequest
{
    public Guid GoalId { get; set; }
}