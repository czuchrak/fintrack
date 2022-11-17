using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;

public class AddPropertyTransactionCommand : RequestBase, IRequest
{
    public PropertyTransactionModel Model { get; set; }
}