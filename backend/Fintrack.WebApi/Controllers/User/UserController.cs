using System;
using System.IO;
using System.Threading.Tasks;
using Fintrack.App.Functions.Profile.Commands.DeleteUser;
using Fintrack.App.Functions.Profile.Commands.GetOrCreateUser;
using Fintrack.App.Functions.Profile.Commands.ImportUserData;
using Fintrack.App.Functions.Profile.Commands.MarkNotificationAsRead;
using Fintrack.App.Functions.Profile.Commands.SendUserMessage;
using Fintrack.App.Functions.Profile.Commands.SetMailVerificationSent;
using Fintrack.App.Functions.Profile.Commands.SetSetting;
using Fintrack.App.Functions.Profile.Models;
using Fintrack.App.Functions.Profile.Queries.ExportUserData;
using Fintrack.App.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.User;

public class UserController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<UserModel> Get()
    {
        return await mediator.Send(new GetOrCreateUserCommand
        {
            Email = GetEmail(HttpContext),
            UserId = UserId
        });
    }

    [HttpDelete]
    public async Task Delete()
    {
        await mediator.Send(new DeleteUserCommand { UserId = UserId });
    }

    [HttpPost]
    [Route("message")]
    public async Task Message(MessageModel message)
    {
        await mediator.Send(new SendUserMessageCommand { Model = message, UserId = UserId });
    }

    [HttpPost]
    [Route("notifications/[action]")]
    public async Task MarkAsRead(Guid notificationId)
    {
        await mediator.Send(new MarkNotificationAsReadCommand { NotificationId = notificationId, UserId = UserId });
    }

    [HttpPost]
    [Route("settings")]
    public async Task Settings(string name, bool value)
    {
        await mediator.Send(new SetSettingCommand { UserId = UserId, Name = name, Value = value });
    }

    [HttpPost]
    [Route("mailVerificationSent")]
    public async Task SetMailVerificationSent()
    {
        await mediator.Send(new SetMailVerificationSentCommand { UserId = UserId });
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var fileModel = await mediator.Send(new ExportUserDataQuery { UserId = UserId });
        return File(fileModel.Content, fileModel.ContentType, fileModel.FileName);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File not selected or empty.");

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileContent = memoryStream.ToArray();

        var result = await mediator.Send(new ImportUserDataCommand { UserId = UserId, FileContent = fileContent });

        return Ok(new
        {
            result.PartsAdded,
            result.EntriesAdded,
            result.PropertiesAdded,
            result.PropertyTransactionsAdded,
            result.Errors
        });
    }
}