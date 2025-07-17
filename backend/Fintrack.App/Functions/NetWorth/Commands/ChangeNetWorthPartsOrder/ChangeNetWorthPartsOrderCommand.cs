using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;

public class ChangeNetWorthPartsOrderCommand : RequestBase, IRequest<Unit>
{
    public IEnumerable<Guid> PartIds { get; set; }
}