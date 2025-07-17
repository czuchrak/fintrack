using Fintrack.App.Functions.NetWorth.Models;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthGoal;

public class UpdateNetWorthGoalCommand : RequestBase, IRequest<Unit>
{
    public NetWorthGoalModel Model { get; set; }
}