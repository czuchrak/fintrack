using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetNotifications;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class GetNotificationsQueryTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
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

    [Test]
    public async Task GetNotificationsQueryHandler_ReturnsAllNotifications()
    {
        await using var context = CreateContext();
        var handler = new GetNotificationsQueryHandler(context);

        var notifications =
            (await handler.Handle(new GetNotificationsQuery { UserId = UserId }, new CancellationToken()))
            .ToList();

        Assert.AreEqual(1, notifications.Count);
        Assert.IsNotEmpty(notifications[0].Id.ToString());
        Assert.IsNotEmpty(notifications[0].Message);
        Assert.IsNotEmpty(notifications[0].Type);
        Assert.IsNotEmpty(notifications[0].Url);
        Assert.NotNull(notifications[0].ValidFrom);
        Assert.NotNull(notifications[0].ValidUntil);
        Assert.AreEqual(true, notifications[0].IsActive);
        Assert.AreEqual(1, notifications[0].UsersCount);
        Assert.AreEqual(1, notifications[0].ReadCount);
    }

    [Test]
    public async Task GetNotificationsQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new GetNotificationsQueryHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new GetNotificationsQuery { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task GetNotificationsQueryValidator_ValidatesUserId()
    {
        var validator = new GetNotificationsQueryValidator();
        var result = await validator.TestValidateAsync(new GetNotificationsQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task GetNotificationsQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetNotificationsQueryValidator();
        var result = await validator.TestValidateAsync(new GetNotificationsQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}