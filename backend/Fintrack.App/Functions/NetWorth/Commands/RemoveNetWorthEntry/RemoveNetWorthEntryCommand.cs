using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthEntry;

public class RemoveNetWorthEntryCommand : RequestBase, IRequest<Unit>
{
    public Guid EntryId { get; set; }
}