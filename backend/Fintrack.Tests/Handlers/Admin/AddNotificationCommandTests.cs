using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.AddNotification;
using Fintrack.App.Functions.Admin.Models;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class AddNotificationCommandTests : TestBase
{
    [Test]
    public async Task AddNotificationCommandHandler_AddNewNotification()
    {
        await using var context = CreateContext();
        var handler = new AddNotificationCommandHandler(context);

        await handler.Handle(new AddNotificationCommand
            {
                Model = new NotificationModel
                {
                    Type = Guid.NewGuid().ToString(),
                    Message = Guid.NewGuid().ToString(),
                    Url = Guid.NewGuid().ToString(),
                    ValidFrom = DateTime.Today,
                    ValidUntil = DateTime.Now,
                    IsActive = true
                },
                UserId = UserId
            },
            new CancellationToken());

        var notifications = await context.Notifications.ToListAsync();

        Assert.AreEqual(1, notifications.Count);
        Assert.IsNotEmpty(notifications[0].Id.ToString());
        Assert.IsNotEmpty(notifications[0].Message);
        Assert.IsNotEmpty(notifications[0].Type);
        Assert.IsNotEmpty(notifications[0].Url);
        Assert.NotNull(notifications[0].ValidFrom);
        Assert.NotNull(notifications[0].ValidUntil);
        Assert.AreEqual(true, notifications[0].IsActive);
        Assert.AreEqual(true, notifications[0].IsActive);
    }

    [Test]
    public async Task AddNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new AddNotificationCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new AddNotificationCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task AddNotificationCommandValidator_ValidatesFields()
    {
        var validator = new AddNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new AddNotificationCommand
            { Model = new NotificationModel { Message = "Test", Type = "Test2" }, UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Message);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
    }

    [Test]
    public async Task AddNotificationCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new AddNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new AddNotificationCommand
            { Model = new NotificationModel(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Message);
        result.ShouldHaveValidationErrorFor(x => x.Model.Type);
    }
}