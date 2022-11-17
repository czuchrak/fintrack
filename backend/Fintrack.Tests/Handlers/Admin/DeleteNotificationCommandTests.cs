using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DeleteNotification;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class DeleteNotificationCommandTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        var id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4");
        await using var context = CreateContext();

        context.Notifications.Add(new Notification
        {
            Id = id,
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
    public async Task DeleteNotificationCommandHandler_DeleteNotification()
    {
        await using var context = CreateContext();
        var handler = new DeleteNotificationCommandHandler(context);

        await handler.Handle(new DeleteNotificationCommand
            {
                NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                UserId = UserId
            },
            new CancellationToken());

        var notifications = await context.Notifications.ToListAsync();

        Assert.AreEqual(0, notifications.Count);
    }

    [Test]
    public async Task DeleteNotificationCommandHandler_ThrowsException_WhenNotificationDoesNotExist()
    {
        await using var context = CreateContext();
        var handler = new DeleteNotificationCommandHandler(context);

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await handler.Handle(new DeleteNotificationCommand
            {
                NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
                UserId = UserId
            }, new CancellationToken()));
    }

    [Test]
    public async Task DeleteNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new DeleteNotificationCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new DeleteNotificationCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task DeleteNotificationCommandValidator_ValidatesFields()
    {
        var validator = new DeleteNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteNotificationCommand
        {
            NotificationId = Guid.NewGuid(),
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.NotificationId);
    }

    [Test]
    public async Task DeleteNotificationCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new DeleteNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteNotificationCommand { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.NotificationId);
    }
}