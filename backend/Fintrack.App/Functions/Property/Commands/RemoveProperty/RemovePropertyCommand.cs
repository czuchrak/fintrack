using MediatR;

namespace Fintrack.App.Functions.Property.Commands.RemoveProperty;

public class RemovePropertyCommand : RequestBase, IRequest
{
    public Guid PropertyId { get; set; }
}