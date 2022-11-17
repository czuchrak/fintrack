using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Queries.GetProperties;

public class GetPropertiesQuery : RequestBase, IRequest<IEnumerable<PropertyModel>>
{
}