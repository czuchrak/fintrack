using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthEntry;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class UpdateNetWorthEntryCommandTests : TestBase
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

        context.ExchangeRates.Add(new ExchangeRate
        {
            Currency = "EUR",
            Date = DateTime.Parse("2022-01-01"),
            Rate = 4.5M
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateNetWorthEntryCommandHandler_UpdatesEntry()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNetWorthEntryCommandHandler(context);

        await handler.Handle(new UpdateNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
                Date = "2022-01-01",
                PartValues = new Dictionary<string, decimal>
                {
                    { "92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4", 2000M }
                }
            },
            UserId = UserId
        }, CancellationToken.None);

        var entry = await context.NetWorthEntries
            .Include(x => x.EntryParts)
            .FirstAsync(x => x.Id == new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"));

        entry.Date.Should().Be(DateTime.Parse("2022-01-01").Date);
        entry.EntryParts.Should().HaveCount(1);
        entry.EntryParts.First().Value.Should().Be(2000M);
    }

    [Fact]
    public async Task UpdateNetWorthEntryCommandHandler_ThrowsException_WhenEntryDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNetWorthEntryCommandHandler(context);

        var act = async () => await handler.Handle(new UpdateNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
                Date = "2022-01-01",
                PartValues = new Dictionary<string, decimal>()
            },
            UserId = UserId
        }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateNetWorthEntryCommandValidator_ValidatesFields()
    {
        var validator = new UpdateNetWorthEntryCommandValidator();
        var result = await validator.TestValidateAsync(new UpdateNetWorthEntryCommand
        {
            Model = new NetWorthEntryModel
            {
                Id = Guid.NewGuid(),
                Date = "2022-01-01",
                PartValues = new Dictionary<string, decimal>()
            },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Date);
    }
}