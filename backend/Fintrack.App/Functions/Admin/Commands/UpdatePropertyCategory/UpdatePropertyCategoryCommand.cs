using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.UpdatePropertyCategory;

public class UpdatePropertyCategoryCommand : RequestBase, IRequest
{
    public PropertyCategoryModel Model { get; set; }
}