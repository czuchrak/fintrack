using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Queries.GetNetWorthModel;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class GetNetWorthModelQueryTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        // Add test data
        context.NetWorthParts.AddRange(
            new NetWorthPart
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                Name = "Cash",
                Type = "Asset",
                Currency = "PLN",
                IsVisible = true,
                Order = 1,
                UserId = UserId
            },
            new NetWorthPart
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
                Name = "Investments",
                Type = "Asset",
                Currency = "USD",
                IsVisible = true,
                Order = 2,
                UserId = UserId
            }
        );

        context.NetWorthEntries.Add(new NetWorthEntry
        {
            Id = Guid.NewGuid(),
            UserId = UserId,
            Date = DateTime.Parse("2022-01-01"),
            ExchangeRateDate = DateTime.Parse("2022-01-01"),
            EntryParts = new List<NetWorthEntryPart>
            {
                new()
                {
                    NetWorthPartId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                    Value = 1000M
                }
            }
        });

        context.NetWorthGoals.Add(new NetWorthGoal
        {
            Id = Guid.NewGuid(),
            UserId = UserId,
            Name = "Test Goal",
            Value = 10000M,
            ReturnRate = 7M,
            Deadline = DateTime.Now.AddYears(1),
            GoalParts = new List<NetWorthGoalPart>
            {
                new()
                {
                    NetWorthPartId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")
                }
            }
        });

        context.ExchangeRates.AddRange(
            new ExchangeRate { Currency = "USD", Date = DateTime.Parse("2022-01-01"), Rate = 4.0M },
            new ExchangeRate { Currency = "EUR", Date = DateTime.Parse("2022-01-01"), Rate = 4.5M }
        );

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetNetWorthModelQueryHandler_ReturnsCompleteModel()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetNetWorthModelQueryHandler(context);

        var result = await handler.Handle(new GetNetWorthModelQuery { UserId = UserId }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Parts.Should().HaveCount(2);
        result.Parts.First().Name.Should().Be("Cash");
        result.Parts.First().Currency.Should().Be("PLN");
        result.Parts.Skip(1).First().Name.Should().Be("Investments");

        result.Entries.Should().HaveCountGreaterThan(0);
        result.Goals.Should().HaveCount(1);
        result.Goals.First().Name.Should().Be("Test Goal");
        result.Goals.First().Value.Should().Be(10000M);

        result.Rates.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNetWorthModelQueryHandler_ReturnsEmptyForNewUser()
    {
        await using var context = CreateContext();
        var handler = new GetNetWorthModelQueryHandler(context);

        var result = await handler.Handle(new GetNetWorthModelQuery { UserId = "new-user-id" }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Parts.Should().BeEmpty();
        result.Entries.Should().BeEmpty();
        result.Goals.Should().BeEmpty();
        result.Rates.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNetWorthModelQueryValidator_ValidatesUserId()
    {
        var validator = new GetNetWorthModelQueryValidator();
        var result = await validator.TestValidateAsync(new GetNetWorthModelQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetNetWorthModelQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetNetWorthModelQueryValidator();
        var result = await validator.TestValidateAsync(new GetNetWorthModelQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}