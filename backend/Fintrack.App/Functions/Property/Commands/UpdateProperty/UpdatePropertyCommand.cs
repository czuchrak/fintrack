using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Commands.UpdateProperty;

public class UpdatePropertyCommand : RequestBase, IRequest<Unit>
{
    public PropertyModel Model { get; set; }
}