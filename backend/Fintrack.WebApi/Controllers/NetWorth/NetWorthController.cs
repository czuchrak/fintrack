using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.App.Functions.NetWorth.Queries.GetNetWorthModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.NetWorth;

public class NetWorthController : BaseController
{
    private readonly IMediator _mediator;

    public NetWorthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<NetWorthModel> Get()
    {
        return await _mediator.Send(new GetNetWorthModelQuery { UserId = UserId });
    }
}