using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.AddNetWorthEntry;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class AddNetWorthEntryCommandTests : TestBase
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

        context.ExchangeRates.Add(new ExchangeRate
        {
            Currency = "EUR",
            Date = DateTime.Parse("2022-01-01"),
            Rate = 4.5M
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddNetWorthEntryCommandHandler_AddNewEntry()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new AddNetWorthEntryCommandHandler(context);

        await handler.Handle(new AddNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel
            {
                Date = "2022-01-01",
                PartValues = new Dictionary<string, decimal>
                {
                    { "92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4", 1000M }
                }
            },
            UserId = UserId
        }, CancellationToken.None);

        var entries = await context.NetWorthEntries.Include(x => x.EntryParts).ToListAsync();

        entries.Should().HaveCount(1);
        entries[0].Date.Should().Be(DateTime.Parse("2022-01-01").Date);
        entries[0].UserId.Should().Be(UserId);
        entries[0].EntryParts.Should().HaveCount(1);
        entries[0].EntryParts.First().NetWorthPartId.Should().Be(new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"));
        entries[0].EntryParts.First().Value.Should().Be(1000M);
    }

    [Fact]
    public async Task AddNetWorthEntryCommandHandler_SkipsZeroValues()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new AddNetWorthEntryCommandHandler(context);

        await handler.Handle(new AddNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel
            {
                Date = "2022-01-01",
                PartValues = new Dictionary<string, decimal>
                {
                    { "92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4", 1000M },
                    { "92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5", 0M }
                }
            },
            UserId = UserId
        }, CancellationToken.None);

        var entries = await context.NetWorthEntries.Include(x => x.EntryParts).ToListAsync();

        entries.Should().HaveCount(1);
        entries[0].EntryParts.Should().HaveCount(1);
        entries[0].EntryParts.First().Value.Should().Be(1000M);
    }

    [Fact]
    public async Task AddNetWorthEntryCommandValidator_ValidatesFields()
    {
        var validator = new AddNetWorthEntryCommandValidator();
        var result = await validator.TestValidateAsync(new AddNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel { Date = "2022-01-01", PartValues = new Dictionary<string, decimal>() },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Date);
    }

    [Fact]
    public async Task AddNetWorthEntryCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new AddNetWorthEntryCommandValidator();
        var result = await validator.TestValidateAsync(new AddNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel(),
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Date);
    }
}