using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Commands.UpdatePropertyTransaction;

public class UpdatePropertyTransactionCommand : RequestBase, IRequest<Unit>
{
    public PropertyTransactionModel Model { get; set; }
}