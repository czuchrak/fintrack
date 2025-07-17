using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class ChangeNetWorthPartsOrderCommandTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();
        context.NetWorthParts.Add(new NetWorthPart
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Name = "Test23",
            Type = "Asset",
            Currency = "PLN",
            IsVisible = true,
            UserId = UserId
        });
        context.NetWorthParts.Add(new NetWorthPart
        {
            Id = new Guid("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD1"),
            Name = "Test12",
            Type = "Liability",
            Currency = "USD",
            IsVisible = false,
            UserId = UserId
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task ChangeNetWorthPartsOrderCommandHandler_ChangeNetWorthPartsOrder()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new ChangeNetWorthPartsOrderCommandHandler(context);

        await handler.Handle(new ChangeNetWorthPartsOrderCommand
            {
                PartIds = new List<Guid>
                {
                    Guid.Parse("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD1"),
                    Guid.Parse("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")
                },
                UserId = UserId
            },
            CancellationToken.None);

        var parts = await context.NetWorthParts.OrderBy(x => x.Order).ToListAsync();

        parts.Should().HaveCount(2);
        parts[0].Id.ToString().ToUpper().Should().Be("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD1");
        parts[1].Id.ToString().ToUpper().Should().Be("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4");
    }

    [Fact]
    public async Task ChangeNetWorthPartsOrderCommandHandler_ThrowsException_WhenNetWorthPartDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new ChangeNetWorthPartsOrderCommandHandler(context);

        var act = async () => await handler.Handle(new ChangeNetWorthPartsOrderCommand
        {
            PartIds = new List<Guid>
            {
                Guid.Parse("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD0"),
                Guid.Parse("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")
            },
            UserId = UserId
        }, new CancellationToken());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ChangeNetWorthPartsOrderCommandValidator_ValidatesFields()
    {
        var validator = new ChangeNetWorthPartsOrderCommandValidator();
        var result = await validator.TestValidateAsync(new ChangeNetWorthPartsOrderCommand
        {
            PartIds = new List<Guid>
            {
                Guid.Parse("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD1"),
                Guid.Parse("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")
            },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.PartIds);
    }

    [Fact]
    public async Task ChangeNetWorthPartsOrderCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new ChangeNetWorthPartsOrderCommandValidator();
        var result = await validator.TestValidateAsync(new ChangeNetWorthPartsOrderCommand
            { PartIds = new List<Guid>(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.PartIds);
    }
}