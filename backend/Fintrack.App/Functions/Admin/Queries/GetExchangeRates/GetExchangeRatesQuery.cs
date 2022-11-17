using Fintrack.App.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Queries.GetExchangeRates;

public class GetExchangeRatesQuery : RequestBase, IRequest<IEnumerable<ExchangeRateModel>>
{
}