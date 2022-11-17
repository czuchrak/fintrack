using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DuplicateNotification;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class DuplicateNotificationCommandTests : TestBase
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
    public async Task DuplicateNotificationCommandHandler_DuplicateNotification()
    {
        await using var context = CreateContext();
        var handler = new DuplicateNotificationCommandHandler(context);

        await handler.Handle(new DuplicateNotificationCommand
            {
                NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                UserId = UserId
            },
            new CancellationToken());

        var notifications = await context.Notifications.ToListAsync();

        Assert.AreEqual(2, notifications.Count);
        Assert.IsNotEmpty(notifications[0].Id.ToString());
        Assert.IsNotEmpty(notifications[0].Message);
        Assert.IsNotEmpty(notifications[0].Type);
        Assert.IsNotEmpty(notifications[0].Url);
        Assert.AreEqual(false, notifications[0].IsActive);
    }

    [Test]
    public async Task DuplicateNotificationCommandHandler_ThrowsException_WhenNotificationDoesNotExist()
    {
        await using var context = CreateContext();
        var handler = new DuplicateNotificationCommandHandler(context);

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await handler.Handle(new DuplicateNotificationCommand
            {
                NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
                UserId = UserId
            }, new CancellationToken()));
    }

    [Test]
    public async Task DuplicateNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new DuplicateNotificationCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new DuplicateNotificationCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task DuplicateNotificationCommandValidator_ValidatesFields()
    {
        var validator = new DuplicateNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new DuplicateNotificationCommand
        {
            NotificationId = Guid.NewGuid(),
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.NotificationId);
    }

    [Test]
    public async Task DuplicateNotificationCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new DuplicateNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new DuplicateNotificationCommand { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.NotificationId);
    }
}