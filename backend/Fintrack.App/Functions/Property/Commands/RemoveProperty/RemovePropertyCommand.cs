using MediatR;

namespace Fintrack.App.Functions.Property.Commands.RemoveProperty;

public class RemovePropertyCommand : RequestBase, IRequest<Unit>
{
    public Guid PropertyId { get; set; }
}