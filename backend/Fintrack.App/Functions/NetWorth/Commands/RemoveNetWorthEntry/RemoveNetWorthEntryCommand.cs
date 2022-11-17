using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthEntry;

public class RemoveNetWorthEntryCommand : RequestBase, IRequest
{
    public Guid EntryId { get; set; }
}