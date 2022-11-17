using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetUsers;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class GetUsersQueryTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        await using var context = CreateContext();

        context.NetWorthParts.Add(new NetWorthPart
        {
            Name = Guid.NewGuid().ToString(),
            UserId = UserId,
            Currency = "PLN",
            Type = "Asset"
        });

        context.NetWorthEntries.Add(new NetWorthEntry
        {
            UserId = UserId
        });

        context.Properties.Add(new Property
        {
            UserId = UserId,
            Name = Guid.NewGuid().ToString()
        });

        context.NetWorthGoals.Add(new NetWorthGoal
        {
            Name = Guid.NewGuid().ToString(),
            Deadline = DateTime.Now,
            Value = 1000,
            ReturnRate = 10,
            UserId = UserId
        });

        await context.SaveChangesAsync();
    }

    [Test]
    public async Task GetUsersQueryHandler_ReturnsAllUsers()
    {
        await using var context = CreateContext();
        var handler = new GetUsersQueryHandler(context);

        var users = (await handler.Handle(new GetUsersQuery { UserId = UserId }, new CancellationToken())).ToList();

        Assert.AreEqual(1, users.Count);
        Assert.IsNotEmpty(users[0].Id);
        Assert.AreEqual("test", users[0].Name);
        Assert.IsNotNull(users[0].CreationDate);
        Assert.IsNotNull(users[0].LastActivity);
        Assert.AreEqual(true, users[0].NewsEmailEnabled);
        Assert.AreEqual(true, users[0].NewMonthEmailEnabled);
        Assert.AreEqual(1, users[0].PartsCount);
        Assert.AreEqual(1, users[0].PropertiesCount);
        Assert.AreEqual(1, users[0].EntriesCount);
        Assert.AreEqual(1, users[0].GoalsCount);
    }

    [Test]
    public async Task GetUsersQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new GetUsersQueryHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new GetUsersQuery { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task GetUsersQueryValidator_ValidatesUserId()
    {
        var validator = new GetUsersQueryValidator();
        var result = await validator.TestValidateAsync(new GetUsersQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task GetUsersQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetUsersQueryValidator();
        var result = await validator.TestValidateAsync(new GetUsersQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}