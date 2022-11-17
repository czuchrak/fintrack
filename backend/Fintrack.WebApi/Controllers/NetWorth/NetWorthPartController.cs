using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.AddNetWorthPart;
using Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;
using Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthPart;
using Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthPart;
using Fintrack.App.Functions.NetWorth.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.NetWorth;

public class NetWorthPartController : BaseController
{
    private readonly IMediator _mediator;

    public NetWorthPartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Post(NetWorthPartModel model)
    {
        await _mediator.Send(new AddNetWorthPartCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(NetWorthPartModel model)
    {
        await _mediator.Send(new UpdateNetWorthPartCommand { Model = model, UserId = UserId });
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new RemoveNetWorthPartCommand { PartId = id, UserId = UserId });
    }

    [HttpPost]
    [Route("[action]")]
    public async Task Order(IEnumerable<Guid> ids)
    {
        await _mediator.Send(new ChangeNetWorthPartsOrderCommand { PartIds = ids, UserId = UserId });
    }
}