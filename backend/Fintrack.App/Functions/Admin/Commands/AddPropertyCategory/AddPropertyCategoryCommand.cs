using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.AddPropertyCategory;

public class AddPropertyCategoryCommand : RequestBase, IRequest<Unit>
{
    public PropertyCategoryModel Model { get; set; }
}