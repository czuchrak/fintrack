using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DuplicateNotification;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class DuplicateNotificationCommandTests : TestBase
{
    private async Task InitializeAsync()
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

    [Fact]
    public async Task DuplicateNotificationCommandHandler_DuplicateNotification()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DuplicateNotificationCommandHandler(context);

        await handler.Handle(new DuplicateNotificationCommand
            {
                NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                UserId = UserId
            },
            CancellationToken.None);

        var notifications = await context.Notifications.ToListAsync();

        notifications.Should().HaveCount(2);
        notifications[0].Id.ToString().Should().NotBeEmpty();
        notifications[0].Message.Should().NotBeEmpty();
        notifications[0].Type.Should().NotBeEmpty();
        notifications[0].Url.Should().NotBeEmpty();
        notifications[0].IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DuplicateNotificationCommandHandler_ThrowsException_WhenNotificationDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DuplicateNotificationCommandHandler(context);

        var act = async () => await handler.Handle(new DuplicateNotificationCommand
        {
            NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
            UserId = UserId
        }, new CancellationToken());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task DuplicateNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DuplicateNotificationCommandHandler(context);

        var act = async () =>
            await handler.Handle(new DuplicateNotificationCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
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

    [Fact]
    public async Task DuplicateNotificationCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new DuplicateNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new DuplicateNotificationCommand { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.NotificationId);
    }
}