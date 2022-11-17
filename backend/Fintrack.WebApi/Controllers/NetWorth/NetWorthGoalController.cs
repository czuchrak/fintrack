using System;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.AddNetWorthGoal;
using Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthGoal;
using Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthGoal;
using Fintrack.App.Functions.NetWorth.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.NetWorth;

public class NetWorthGoalController : BaseController
{
    private readonly IMediator _mediator;

    public NetWorthGoalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Post(NetWorthGoalModel model)
    {
        await _mediator.Send(new AddNetWorthGoalCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(NetWorthGoalModel model)
    {
        await _mediator.Send(new UpdateNetWorthGoalCommand { Model = model, UserId = UserId });
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new RemoveNetWorthGoalCommand { GoalId = id, UserId = UserId });
    }
}