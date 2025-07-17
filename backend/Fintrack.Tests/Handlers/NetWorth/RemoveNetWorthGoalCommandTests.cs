using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthGoal;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class RemoveNetWorthGoalCommandTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        context.NetWorthParts.Add(new NetWorthPart
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Name = "Test Part",
            Type = "Asset",
            Currency = "PLN",
            IsVisible = true,
            UserId = UserId,
            Order = 1
        });

        context.NetWorthGoals.Add(new NetWorthGoal
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
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

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveNetWorthGoalCommandHandler_RemovesGoal()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new RemoveNetWorthGoalCommandHandler(context);

        await handler.Handle(new RemoveNetWorthGoalCommand
        {
            GoalId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
            UserId = UserId
        }, CancellationToken.None);

        var goals = await context.NetWorthGoals.ToListAsync();
        goals.Should().HaveCount(0);
    }

    [Fact]
    public async Task RemoveNetWorthGoalCommandHandler_ThrowsException_WhenGoalDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new RemoveNetWorthGoalCommandHandler(context);

        var act = async () => await handler.Handle(new RemoveNetWorthGoalCommand
        {
            GoalId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
            UserId = UserId
        }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RemoveNetWorthGoalCommandValidator_ValidatesFields()
    {
        var validator = new RemoveNetWorthGoalCommandValidator();
        var result = await validator.TestValidateAsync(new RemoveNetWorthGoalCommand
        {
            GoalId = Guid.NewGuid(),
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.GoalId);
    }

    [Fact]
    public async Task RemoveNetWorthGoalCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new RemoveNetWorthGoalCommandValidator();
        var result = await validator.TestValidateAsync(new RemoveNetWorthGoalCommand
        {
            GoalId = Guid.Empty,
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.GoalId);
    }
}