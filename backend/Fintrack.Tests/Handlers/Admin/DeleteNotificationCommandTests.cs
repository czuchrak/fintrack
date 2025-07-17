using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DeleteNotification;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class DeleteNotificationCommandTests : TestBase
{
    private async Task InitializeAsync()
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

    [Fact]
    public async Task DeleteNotificationCommandHandler_DeleteNotification()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DeleteNotificationCommandHandler(context);

        await handler.Handle(new DeleteNotificationCommand
            {
                NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                UserId = UserId
            },
            CancellationToken.None);

        var notifications = await context.Notifications.ToListAsync();

        notifications.Should().HaveCount(0);
    }

    [Fact]
    public async Task DeleteNotificationCommandHandler_ThrowsException_WhenNotificationDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DeleteNotificationCommandHandler(context);

        var act = async () => await handler.Handle(new DeleteNotificationCommand
        {
            NotificationId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
            UserId = UserId
        }, new CancellationToken());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task DeleteNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DeleteNotificationCommandHandler(context);

        var act = async () =>
            await handler.Handle(new DeleteNotificationCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
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

    [Fact]
    public async Task DeleteNotificationCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new DeleteNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteNotificationCommand { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.NotificationId);
    }
}