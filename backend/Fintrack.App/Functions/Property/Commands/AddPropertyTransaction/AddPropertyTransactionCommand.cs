using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;

public class AddPropertyTransactionCommand : RequestBase, IRequest<Unit>
{
    public PropertyTransactionModel Model { get; init; }
}