using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.UpdateNotification;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class UpdateNotificationCommandTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        await using var context = CreateContext();
        context.Notifications.Add(new Notification
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Type = Guid.NewGuid().ToString(),
            Message = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString(),
            ValidFrom = DateTime.Today,
            ValidUntil = DateTime.Now,
            IsActive = false
        });

        await context.SaveChangesAsync();
    }

    [Test]
    public async Task UpdateNotificationCommandHandler_UpdateNotification()
    {
        await using var context = CreateContext();
        var handler = new UpdateNotificationCommandHandler(context);

        await handler.Handle(new UpdateNotificationCommand
            {
                Model = new NotificationModel
                {
                    Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                    Type = "Test11",
                    Message = "Test23",
                    Url = "Url",
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
        Assert.AreEqual("Test23", notifications[0].Message);
        Assert.AreEqual("Test11", notifications[0].Type);
        Assert.AreEqual("Url", notifications[0].Url);
        Assert.AreEqual(true, notifications[0].IsActive);
    }

    [Test]
    public async Task UpdateNotificationCommandHandler_ThrowsException_WhenNotificationDoesNotExist()
    {
        await using var context = CreateContext();
        var handler = new UpdateNotificationCommandHandler(context);

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await handler.Handle(new UpdateNotificationCommand
            {
                Model = new NotificationModel
                {
                    Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0")
                },
                UserId = UserId
            }, new CancellationToken()));
    }

    [Test]
    public async Task UpdateNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new UpdateNotificationCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new UpdateNotificationCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task UpdateNotificationCommandValidator_ValidatesFields()
    {
        var validator = new UpdateNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new UpdateNotificationCommand
        {
            Model = new NotificationModel { Id = Guid.NewGuid(), Message = "Test", Type = "Test2" },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Message);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
    }

    [Test]
    public async Task UpdateNotificationCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new UpdateNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new UpdateNotificationCommand
            { Model = new NotificationModel(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldHaveValidationErrorFor(x => x.Model.Message);
        result.ShouldHaveValidationErrorFor(x => x.Model.Type);
    }
}