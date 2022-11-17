using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.NetWorth;

[TestFixture]
public class ChangeNetWorthPartsOrderCommandTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
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

    [Test]
    public async Task ChangeNetWorthPartsOrderCommandHandler_ChangeNetWorthPartsOrder()
    {
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
            new CancellationToken());

        var parts = await context.NetWorthParts.OrderBy(x => x.Order).ToListAsync();

        Assert.AreEqual(2, parts.Count);
        Assert.AreEqual("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD1", parts[0].Id.ToString().ToUpper());
        Assert.AreEqual("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4", parts[1].Id.ToString().ToUpper());
    }

    [Test]
    public async Task ChangeNetWorthPartsOrderCommandHandler_ThrowsException_WhenNetWorthPartDoesNotExist()
    {
        await using var context = CreateContext();
        var handler = new ChangeNetWorthPartsOrderCommandHandler(context);

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await handler.Handle(new ChangeNetWorthPartsOrderCommand
            {
                PartIds = new List<Guid>
                {
                    Guid.Parse("3CEA445A-A9D7-4711-AF85-8CC37B1EEBD0"),
                    Guid.Parse("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4")
                },
                UserId = UserId
            }, new CancellationToken()));
    }

    [Test]
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

    [Test]
    public async Task ChangeNetWorthPartsOrderCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new ChangeNetWorthPartsOrderCommandValidator();
        var result = await validator.TestValidateAsync(new ChangeNetWorthPartsOrderCommand
            { PartIds = new List<Guid>(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.PartIds);
    }
}