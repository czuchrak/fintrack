using Fintrack.App.Functions.NetWorth.Models;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthEntry;

public class UpdateNetWorthEntryCommand : RequestBase, IRequest
{
    public NetWorthEntryModel Model { get; set; }
}