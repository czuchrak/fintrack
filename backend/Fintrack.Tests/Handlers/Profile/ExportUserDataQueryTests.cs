using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Profile.Queries.ExportUserData;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Profile;

public class ExportUserDataQueryTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        var propertyCategory = new PropertyCategory { Id = Guid.NewGuid(), Name = "Salary", Type = "Income" };
        context.PropertyCategories.Add(propertyCategory);

        var property = new Database.Entities.Property
            { Id = Guid.NewGuid(), Name = "My Job", UserId = UserId, IsActive = true };
        context.Properties.Add(property);

        context.PropertyTransactions.Add(new PropertyTransaction
        {
            PropertyId = property.Id,
            CategoryId = propertyCategory.Id,
            Value = 5000,
            Date = new DateTime(2023, 1, 15, 15, 0, 0),
            Details = "Monthly Paycheck"
        });

        var part1 = new NetWorthPart
        {
            Id = Guid.NewGuid(), Name = "Checking Account", Currency = "USD", Type = "Asset", UserId = UserId, Order = 1
        };
        var part2 = new NetWorthPart
        {
            Id = Guid.NewGuid(), Name = "Credit Card", Currency = "USD", Type = "Liability", UserId = UserId, Order = 2
        };
        context.NetWorthParts.AddRange(part1, part2);

        context.NetWorthEntries.Add(new NetWorthEntry
        {
            UserId = UserId,
            Date = new DateTime(2023, 1, 31),
            EntryParts = new[]
            {
                new NetWorthEntryPart { NetWorthPartId = part1.Id, Value = 2500 },
                new NetWorthEntryPart { NetWorthPartId = part2.Id, Value = 500 }
            }
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task ExportUserDataQueryHandler_ReturnsCorrectCsvData()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new ExportUserDataQueryHandler(context);

        var result = await handler.Handle(new ExportUserDataQuery { UserId = UserId }, CancellationToken.None);

        result.Should().NotBeNull();
        result.ContentType.Should().Be("text/csv");
        result.FileName.Should().StartWith("fintrack_export");

        using var stream = new MemoryStream(result.Content);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var csvContent = await reader.ReadToEndAsync();

        csvContent.Should().Contain("## Net Worth");
        csvContent.Should().Contain("Date,Checking Account (USD|Asset),Credit Card (USD|Liability)");
        csvContent.Should().Contain("2023-01-31,2500,500");
        csvContent.Should().Contain("## Properties");
        csvContent.Should().Contain("Property Name,Category Type,Value,Date,Description");
        csvContent.Should().Contain("My Job,Income,5000,2023-01-15 15:00:00,Monthly Paycheck");
    }

    [Fact]
    public async Task ExportUserDataQueryValidator_ValidatesUserId()
    {
        var validator = new ExportUserDataQueryValidator();
        var result = await validator.TestValidateAsync(new ExportUserDataQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task ExportUserDataQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new ExportUserDataQueryValidator();
        var result = await validator.TestValidateAsync(new ExportUserDataQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}