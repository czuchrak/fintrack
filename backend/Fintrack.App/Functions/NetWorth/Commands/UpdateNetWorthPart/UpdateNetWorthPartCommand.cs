using Fintrack.App.Functions.NetWorth.Models;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthPart;

public class UpdateNetWorthPartCommand : RequestBase, IRequest
{
    public NetWorthPartModel Model { get; set; }
}