using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;

public class ChangeNetWorthPartsOrderCommand : RequestBase, IRequest
{
    public IEnumerable<Guid> PartIds { get; set; }
}