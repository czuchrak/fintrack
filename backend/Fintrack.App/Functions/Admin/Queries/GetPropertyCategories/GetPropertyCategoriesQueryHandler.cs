using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Queries.GetPropertyCategories;

public class GetPropertyCategoriesQueryHandler : AdminBaseHandler,
    IRequestHandler<GetPropertyCategoriesQuery, IEnumerable<PropertyCategoryModel>>
{
    public GetPropertyCategoriesQueryHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PropertyCategoryModel>> Handle(GetPropertyCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);

        var transactions = await Context.PropertyTransactions
            .Select(x => x.CategoryId).ToListAsync(cancellationToken);

        return (await Context.PropertyCategories.ToListAsync(cancellationToken))
            .Select(x => new PropertyCategoryModel
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                IsCost = x.IsCost,
                Count = transactions.Count(z => z.ToString() == x.Id.ToString())
            })
            .OrderBy(x => x.Name);
    }
}