using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.UpdateNotification;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class UpdateNotificationCommandTests : TestBase
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
    public async Task UpdateNotificationCommandHandler_UpdateNotification()
    {
        await InitializeAsync();
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
            CancellationToken.None);

        var notifications = await context.Notifications.ToListAsync();

        notifications.Should().HaveCount(1);
        notifications[0].Id.ToString().Should().NotBeEmpty();
        notifications[0].Message.Should().Be("Test23");
        notifications[0].Type.Should().Be("Test11");
        notifications[0].Url.Should().Be("Url");
        notifications[0].IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateNotificationCommandHandler_ThrowsException_WhenNotificationDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNotificationCommandHandler(context);

        var act = async () => await handler.Handle(new UpdateNotificationCommand
        {
            Model = new NotificationModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0")
            },
            UserId = UserId
        }, new CancellationToken());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNotificationCommandHandler(context);

        var act = async () =>
            await handler.Handle(new UpdateNotificationCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
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

    [Fact]
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