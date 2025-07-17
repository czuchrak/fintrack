using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthPart;

public class RemoveNetWorthPartCommand : RequestBase, IRequest<Unit>
{
    public Guid PartId { get; set; }
}