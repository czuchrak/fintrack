using System;
using System.Threading.Tasks;
using Fintrack.App.Functions.Profile.Commands.DeleteUser;
using Fintrack.App.Functions.Profile.Commands.GetOrCreateUser;
using Fintrack.App.Functions.Profile.Commands.MarkNotificationAsRead;
using Fintrack.App.Functions.Profile.Commands.SendUserMessage;
using Fintrack.App.Functions.Profile.Commands.SetMailVerificationSent;
using Fintrack.App.Functions.Profile.Commands.SetSetting;
using Fintrack.App.Functions.Profile.Models;
using Fintrack.App.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.User;

public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<UserModel> Get()
    {
        return await _mediator.Send(new GetOrCreateUserCommand
        {
            Email = GetEmail(HttpContext),
            UserId = UserId
        });
    }

    [HttpDelete]
    public async Task Delete()
    {
        await _mediator.Send(new DeleteUserCommand { UserId = UserId });
    }

    [HttpPost]
    [Route("message")]
    public async Task Message(MessageModel message)
    {
        await _mediator.Send(new SendUserMessageCommand { Model = message, UserId = UserId });
    }

    [HttpPost]
    [Route("notifications/[action]")]
    public async Task MarkAsRead(Guid notificationId)
    {
        await _mediator.Send(new MarkNotificationAsReadCommand { NotificationId = notificationId, UserId = UserId });
    }

    [HttpPost]
    [Route("settings")]
    public async Task Settings(string name, bool value)
    {
        await _mediator.Send(new SetSettingCommand { UserId = UserId, Name = name, Value = value });
    }

    [HttpPost]
    [Route("mailVerificationSent")]
    public async Task SetMailVerificationSent()
    {
        await _mediator.Send(new SetMailVerificationSentCommand { UserId = UserId });
    }
}