using Fintrack.App.Functions.Property.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Queries.GetProperties;

public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyModel>>
{
    private readonly DatabaseContext _context;

    public GetPropertiesQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PropertyModel>> Handle(GetPropertiesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        return await _context.Properties
            .Where(x => x.UserId == userId)
            .Select(x => new PropertyModel
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}