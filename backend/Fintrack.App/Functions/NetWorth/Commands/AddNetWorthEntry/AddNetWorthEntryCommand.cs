using Fintrack.App.Functions.NetWorth.Models;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthEntry;

public class AddNetWorthEntryCommand : RequestBase, IRequest<Unit>
{
    public NetWorthEntryModel Model { get; set; }
}