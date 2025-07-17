using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthGoal;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class UpdateNetWorthGoalCommandTests : TestBase
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
            Name = "Original Goal",
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
    public async Task UpdateNetWorthGoalCommandHandler_UpdatesGoal()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNetWorthGoalCommandHandler(context);

        await handler.Handle(new UpdateNetWorthGoalCommand
        {
            Model = new NetWorthGoalModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
                Name = "Updated Goal",
                Value = 15000M,
                ReturnRate = 8M,
                Deadline = DateTime.Now.AddYears(2),
                Parts = [Guid.Parse("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")]
            },
            UserId = UserId
        }, CancellationToken.None);

        var goal = await context.NetWorthGoals
            .Include(x => x.GoalParts)
            .FirstAsync(x => x.Id == new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"));

        goal.Name.Should().Be("Updated Goal");
        goal.Value.Should().Be(15000M);
        goal.ReturnRate.Should().Be(8M);
        goal.GoalParts.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateNetWorthGoalCommandHandler_ThrowsException_WhenGoalDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNetWorthGoalCommandHandler(context);

        var act = async () => await handler.Handle(new UpdateNetWorthGoalCommand
        {
            Model = new NetWorthGoalModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
                Name = "Non-existent Goal"
            },
            UserId = UserId
        }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateNetWorthGoalCommandValidator_ValidatesFields()
    {
        var validator = new UpdateNetWorthGoalCommandValidator();
        var result = await validator.TestValidateAsync(new UpdateNetWorthGoalCommand
        {
            Model = new NetWorthGoalModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Goal",
                Value = 10000M,
                ReturnRate = 7.5M,
                Deadline = DateTime.Now.AddYears(1)
            },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Value);
    }
}