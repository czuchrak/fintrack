using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.AddNotification;
using Fintrack.App.Functions.Admin.Commands.DeleteNotification;
using Fintrack.App.Functions.Admin.Commands.DuplicateNotification;
using Fintrack.App.Functions.Admin.Commands.UpdateNotification;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.App.Functions.Admin.Queries.GetNotifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Admin;

[Route("api/admin/[controller]")]
public class NotificationsController : BaseController
{
    private readonly IMediator _mediator;

    public NotificationsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<NotificationModel>> Get()
    {
        return await _mediator.Send(new GetNotificationsQuery { UserId = UserId });
    }

    [HttpPost]
    public async Task Post(NotificationModel model)
    {
        await _mediator.Send(new AddNotificationCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(NotificationModel model)
    {
        await _mediator.Send(new UpdateNotificationCommand { Model = model, UserId = UserId });
    }

    [HttpPost]
    [Route("[action]")]
    public async Task Duplicate(Guid id)
    {
        await _mediator.Send(new DuplicateNotificationCommand { NotificationId = id, UserId = UserId });
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new DeleteNotificationCommand { NotificationId = id, UserId = UserId });
    }
}