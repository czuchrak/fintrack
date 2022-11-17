using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetExchangeRates;
using Fintrack.App.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Admin;

[Route("api/admin/[controller]")]
public class RatesController : BaseController
{
    private readonly IMediator _mediator;

    public RatesController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<ExchangeRateModel>> GetRates()
    {
        return await _mediator.Send(new GetExchangeRatesQuery { UserId = UserId });
    }
}