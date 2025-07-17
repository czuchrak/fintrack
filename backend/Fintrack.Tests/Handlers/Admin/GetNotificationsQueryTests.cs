using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetNotifications;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class GetNotificationsQueryTests : TestBase
{
    private async Task InitializeAsync()
    {
        var id = Guid.NewGuid();
        await using var context = CreateContext();
        context.Notifications.Add(new Notification
        {
            Id = id,
            Message = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString(),
            IsActive = true,
            ValidFrom = DateTime.Today,
            ValidUntil = DateTime.Now,
            Type = Guid.NewGuid().ToString()
        });

        context.UserNotifications.Add(new UserNotification
        {
            NotificationId = id,
            UserId = UserId
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetNotificationsQueryHandler_ReturnsAllNotifications()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetNotificationsQueryHandler(context);

        var notifications =
            (await handler.Handle(new GetNotificationsQuery { UserId = UserId }, CancellationToken.None))
            .ToList();

        notifications.Should().HaveCount(1);
        notifications[0].Id.ToString().Should().NotBeEmpty();
        notifications[0].Message.Should().NotBeEmpty();
        notifications[0].Type.Should().NotBeEmpty();
        notifications[0].Url.Should().NotBeEmpty();
        notifications[0].ValidFrom.Should().NotBe(default);
        notifications[0].ValidUntil.Should().NotBe(default);
        notifications[0].IsActive.Should().BeTrue();
        notifications[0].UsersCount.Should().Be(1);
        notifications[0].ReadCount.Should().Be(1);
    }

    [Fact]
    public async Task GetNotificationsQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetNotificationsQueryHandler(context);

        var act = async () =>
            await handler.Handle(new GetNotificationsQuery { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetNotificationsQueryValidator_ValidatesUserId()
    {
        var validator = new GetNotificationsQueryValidator();
        var result = await validator.TestValidateAsync(new GetNotificationsQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetNotificationsQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetNotificationsQueryValidator();
        var result = await validator.TestValidateAsync(new GetNotificationsQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}