using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthEntry;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class RemoveNetWorthEntryCommandTests : TestBase
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

        context.NetWorthEntries.Add(new NetWorthEntry
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
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

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveNetWorthEntryCommandHandler_RemovesEntry()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new RemoveNetWorthEntryCommandHandler(context);

        await handler.Handle(new RemoveNetWorthEntryCommand
        {
            EntryId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
            UserId = UserId
        }, CancellationToken.None);

        var entries = await context.NetWorthEntries.ToListAsync();
        entries.Should().HaveCount(0);
    }

    [Fact]
    public async Task RemoveNetWorthEntryCommandHandler_ThrowsException_WhenEntryDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new RemoveNetWorthEntryCommandHandler(context);

        var act = async () => await handler.Handle(new RemoveNetWorthEntryCommand
        {
            EntryId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
            UserId = UserId
        }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RemoveNetWorthEntryCommandValidator_ValidatesFields()
    {
        var validator = new RemoveNetWorthEntryCommandValidator();
        var result = await validator.TestValidateAsync(new RemoveNetWorthEntryCommand
        {
            EntryId = Guid.NewGuid(),
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.EntryId);
    }

    [Fact]
    public async Task RemoveNetWorthEntryCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new RemoveNetWorthEntryCommandValidator();
        var result = await validator.TestValidateAsync(new RemoveNetWorthEntryCommand
        {
            EntryId = Guid.Empty,
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.EntryId);
    }
}