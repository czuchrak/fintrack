using MediatR;

namespace Fintrack.App.Functions.Property.Commands.RemovePropertyTransaction;

public class RemovePropertyTransactionCommand : RequestBase, IRequest
{
    public Guid PropertyId { get; set; }
    public Guid TransactionId { get; set; }
}