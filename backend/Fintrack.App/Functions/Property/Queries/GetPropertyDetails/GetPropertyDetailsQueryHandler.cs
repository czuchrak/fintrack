using Fintrack.App.Functions.Admin.Models;
using Fintrack.App.Functions.Property.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Queries.GetPropertyDetails;

public class GetPropertyDetailsQueryHandler : IRequestHandler<GetPropertyDetailsQuery, PropertyDetailsModel>
{
    private readonly DatabaseContext _context;

    public GetPropertyDetailsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<PropertyDetailsModel> Handle(GetPropertyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var propertyId = request.PropertyId;

        var property = await _context.Properties
            .SingleAsync(x => x.Id == propertyId && x.UserId == userId, cancellationToken);
        var transactions = await GetTransactions(propertyId, cancellationToken);
        var categories = await GetCategories(cancellationToken);

        return new PropertyDetailsModel
        {
            Id = property.Id,
            Name = property.Name,
            IsActive = property.IsActive,
            Transactions = transactions,
            Categories = categories
        };
    }

    private async Task<IEnumerable<PropertyTransactionModel>> GetTransactions(Guid propertyId,
        CancellationToken cancellationToken)
    {
        return await _context.PropertyTransactions.Where(x => x.PropertyId == propertyId).Select(x =>
            new PropertyTransactionModel
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                PropertyId = x.PropertyId,
                Date = x.Date,
                Value = x.Value,
                Details = x.Details
            }).ToListAsync(cancellationToken);
    }

    private async Task<IEnumerable<PropertyCategoryModel>> GetCategories(CancellationToken cancellationToken)
    {
        return await _context.PropertyCategories.Select(x => new PropertyCategoryModel
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
            IsCost = x.IsCost
        }).ToListAsync(cancellationToken);
    }
}