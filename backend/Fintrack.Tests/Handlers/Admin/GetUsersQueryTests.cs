using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetUsers;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class GetUsersQueryTests : TestBase
{
    private async Task InitializeAsync()
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

        context.Properties.Add(new Database.Entities.Property
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

    [Fact]
    public async Task GetUsersQueryHandler_ReturnsAllUsers()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetUsersQueryHandler(context);

        var users = (await handler.Handle(new GetUsersQuery { UserId = UserId }, CancellationToken.None)).ToList();

        users.Should().HaveCount(1);
        users[0].Id.Should().NotBeEmpty();
        users[0].Name.Should().Be("test");
        users[0].CreationDate.Should().NotBe(default);
        users[0].LastActivity.Should().NotBe(default);
        users[0].NewsEmailEnabled.Should().BeTrue();
        users[0].NewMonthEmailEnabled.Should().BeTrue();
        users[0].PartsCount.Should().Be(1);
        users[0].PropertiesCount.Should().Be(1);
        users[0].EntriesCount.Should().Be(1);
        users[0].GoalsCount.Should().Be(1);
    }

    [Fact]
    public async Task GetUsersQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetUsersQueryHandler(context);

        var act = async () => await handler.Handle(new GetUsersQuery { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetUsersQueryValidator_ValidatesUserId()
    {
        var validator = new GetUsersQueryValidator();
        var result = await validator.TestValidateAsync(new GetUsersQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetUsersQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetUsersQueryValidator();
        var result = await validator.TestValidateAsync(new GetUsersQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}