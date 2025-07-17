using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.AddNetWorthGoal;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class AddNetWorthGoalCommandTests : TestBase
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

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddNetWorthGoalCommandHandler_AddNewGoal()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new AddNetWorthGoalCommandHandler(context);

        await handler.Handle(new AddNetWorthGoalCommand
        {
            Model = new NetWorthGoalModel
            {
                Name = "Test Goal",
                Value = 10000M,
                ReturnRate = 7.5M,
                Deadline = DateTime.Now.AddYears(1),
                Parts = [Guid.Parse("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")]
            },
            UserId = UserId
        }, CancellationToken.None);

        var goals = await context.NetWorthGoals.Include(x => x.GoalParts).ToListAsync();

        goals.Should().HaveCount(1);
        goals[0].Name.Should().Be("Test Goal");
        goals[0].Value.Should().Be(10000M);
        goals[0].ReturnRate.Should().Be(7.5M);
        goals[0].UserId.Should().Be(UserId);
        goals[0].GoalParts.Should().HaveCount(1);
        goals[0].GoalParts.First().NetWorthPartId.Should().Be(new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"));
    }

    [Fact]
    public async Task AddNetWorthGoalCommandValidator_ValidatesFields()
    {
        var validator = new AddNetWorthGoalCommandValidator();
        var result = await validator.TestValidateAsync(new AddNetWorthGoalCommand
        {
            Model = new NetWorthGoalModel
            {
                Name = "Test Goal",
                Value = 10000M,
                ReturnRate = 7.5M,
                Deadline = DateTime.Now.AddYears(1)
            },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Value);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.ReturnRate);
    }
}