using Fintrack.App.Functions.NetWorth.Models;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthGoal;

public class AddNetWorthGoalCommand : RequestBase, IRequest
{
    public NetWorthGoalModel Model { get; set; }
}