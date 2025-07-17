using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.AddNotification;
using Fintrack.App.Functions.Admin.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class AddNotificationCommandTests : TestBase
{
    [Fact]
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
            CancellationToken.None);

        var notifications = await context.Notifications.ToListAsync();

        notifications.Should().HaveCount(1);
        notifications[0].Id.ToString().Should().NotBeEmpty();
        notifications[0].Message.Should().NotBeEmpty();
        notifications[0].Type.Should().NotBeEmpty();
        notifications[0].Url.Should().NotBeEmpty();
        notifications[0].ValidFrom.Should().NotBe(default);
        notifications[0].ValidUntil.Should().NotBe(default);
        notifications[0].IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task AddNotificationCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new AddNotificationCommandHandler(context);

        var act = async () =>
            await handler.Handle(new AddNotificationCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task AddNotificationCommandValidator_ValidatesFields()
    {
        var validator = new AddNotificationCommandValidator();
        var result = await validator.TestValidateAsync(new AddNotificationCommand
            { Model = new NotificationModel { Message = "Test", Type = "Test2" }, UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Message);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
    }

    [Fact]
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