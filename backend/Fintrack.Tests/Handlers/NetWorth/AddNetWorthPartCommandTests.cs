using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.AddNetWorthPart;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class AddNetWorthPartCommandTests : TestBase
{
    [Fact]
    public async Task AddNetWorthPartCommandHandler_AddNewNetWorthPart()
    {
        await using var context = CreateContext();

        var handler = new AddNetWorthPartCommandHandler(context);

        await handler.Handle(new AddNetWorthPartCommand
        {
            Model = new NetWorthPartModel
            {
                Name = Guid.NewGuid().ToString(),
                Type = "Asset",
                Currency = "PLN",
                IsVisible = true
            },
            UserId = UserId
        }, CancellationToken.None);

        var parts = await context.NetWorthParts.ToListAsync();

        parts.Should().HaveCount(1);
        parts[0].Id.ToString().Should().NotBeEmpty();
        parts[0].Name.Should().NotBeEmpty();
        parts[0].Type.Should().Be("Asset");
        parts[0].Currency.Should().Be("PLN");
        parts[0].IsVisible.Should().BeTrue();
        parts[0].Order.Should().Be(1);
    }

    [Fact]
    public async Task AddNetWorthPartCommandHandler_AddNewNetWorthPartWithCorrectOrder()
    {
        await using var context = CreateContext();
        context.RemoveRange(await context.NetWorthParts.ToListAsync());
        context.NetWorthParts.Add(new NetWorthPart
        {
            Name = Guid.NewGuid().ToString(),
            Type = "Asset",
            Currency = "PLN",
            IsVisible = true,
            UserId = UserId,
            Order = 1
        });
        await context.SaveChangesAsync();

        var handler = new AddNetWorthPartCommandHandler(context);

        await handler.Handle(new AddNetWorthPartCommand
        {
            Model = new NetWorthPartModel
            {
                Name = Guid.NewGuid().ToString(),
                Type = "Asset",
                Currency = "PLN",
                IsVisible = true
            },
            UserId = UserId
        }, CancellationToken.None);

        var parts = await context.NetWorthParts.OrderBy(x => x.Order).ToListAsync();

        parts.Should().HaveCount(2);
        parts[0].Order.Should().Be(1);
        parts[1].Order.Should().Be(2);
    }

    [Fact]
    public async Task AddNetWorthPartCommandValidator_ValidatesFields()
    {
        var validator = new AddNetWorthPartCommandValidator();
        var result = await validator.TestValidateAsync(new AddNetWorthPartCommand
            { Model = new NetWorthPartModel { Name = "Test", Type = "Test2", Currency = "PLN" }, UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Currency);
    }

    [Fact]
    public async Task AddNetWorthPartCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new AddNetWorthPartCommandValidator();
        var result = await validator.TestValidateAsync(new AddNetWorthPartCommand
            { Model = new NetWorthPartModel(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldHaveValidationErrorFor(x => x.Model.Type);
        result.ShouldHaveValidationErrorFor(x => x.Model.Currency);
    }
}