using Fintrack.App.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Queries.GetExchangeRates;

public class GetExchangeRatesQueryHandler : AdminBaseHandler,
    IRequestHandler<GetExchangeRatesQuery, IEnumerable<ExchangeRateModel>>
{
    public GetExchangeRatesQueryHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ExchangeRateModel>> Handle(GetExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);

        return await Context.ExchangeRates
            .Select(x => new ExchangeRateModel
            {
                Currency = x.Currency,
                Rate = x.Rate,
                Date = x.Date
            })
            .ToListAsync(cancellationToken);
    }
}