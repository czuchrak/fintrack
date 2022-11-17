using System;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.AddNetWorthEntry;
using Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthEntry;
using Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthEntry;
using Fintrack.App.Functions.NetWorth.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.NetWorth;

public class NetWorthEntryController : BaseController
{
    private readonly IMediator _mediator;

    public NetWorthEntryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Post(NetWorthEntryModel model)
    {
        await _mediator.Send(new AddNetWorthEntryCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(NetWorthEntryModel model)
    {
        await _mediator.Send(new UpdateNetWorthEntryCommand { Model = model, UserId = UserId });
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new RemoveNetWorthEntryCommand { EntryId = id, UserId = UserId });
    }
}