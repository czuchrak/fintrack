using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Queries.GetPropertyCategories;

public class GetPropertyCategoriesQuery : RequestBase, IRequest<IEnumerable<PropertyCategoryModel>>
{
}