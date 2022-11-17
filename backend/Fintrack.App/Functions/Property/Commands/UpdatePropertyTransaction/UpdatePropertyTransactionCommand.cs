using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Commands.UpdatePropertyTransaction;

public class UpdatePropertyTransactionCommand : RequestBase, IRequest
{
    public PropertyTransactionModel Model { get; set; }
}