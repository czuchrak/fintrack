using Fintrack.App.Functions.Property.Models;
using MediatR;

namespace Fintrack.App.Functions.Property.Queries.GetPropertyDetails;

public class GetPropertyDetailsQuery : RequestBase, IRequest<PropertyDetailsModel>
{
    public Guid PropertyId { get; set; }
}