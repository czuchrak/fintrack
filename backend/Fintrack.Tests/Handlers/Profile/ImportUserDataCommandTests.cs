using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Profile.Commands.ImportUserData;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Fintrack.Tests.Handlers.Profile;

public class ImportUserDataCommandTests : TestBase
{
    private const string CsvContent = @"## Net Worth
Date,Checking Account (USD|asset),Credit Card (USD|liability)
2023-01-31,2500,500

## Properties
Property Name,Category Type,Value,Date,Description
My Job,Income,5000,2023-01-15,Monthly Paycheck
";

    [Fact]
    public async Task ImportUserDataCommandHandler_ImportsDataCorrectly()
    {
        await using var context = CreateContext();

        await context.PropertyCategories.AddAsync(new PropertyCategory
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B6"),
            Name = "Salary",
            Type = "Income",
            IsCost = false
        });

        await context.ExchangeRates.AddAsync(new ExchangeRate
        {
            Currency = "USD",
            Rate = 1.0m,
            Date = DateTime.Parse("2023-01-31")
        });

        var logger = new LoggerFactory().CreateLogger<ImportUserDataCommandHandler>();
        var handler = new ImportUserDataCommandHandler(context, logger);
        var command = new ImportUserDataCommand
        {
            UserId = UserId,
            FileContent = Encoding.UTF8.GetBytes(CsvContent)
        };

        await handler.Handle(command, CancellationToken.None);

        var netWorthParts = await context.NetWorthParts.Where(x => x.UserId == UserId).ToListAsync();
        netWorthParts.Should().HaveCount(2);
        netWorthParts.Should().Contain(p => p.Name == "Checking Account" && p.Type == "asset");
        netWorthParts.Should().Contain(p => p.Name == "Credit Card" && p.Type == "liability");

        var netWorthEntries = await context.NetWorthEntries.Where(x => x.UserId == UserId).Include(e => e.EntryParts)
            .ToListAsync();
        netWorthEntries.Should().HaveCount(1);
        netWorthEntries[0].EntryParts.Should().HaveCount(2);

        var properties = await context.Properties.Where(x => x.UserId == UserId).ToListAsync();
        properties.Should().HaveCount(1);
        properties[0].Name.Should().Be("My Job");

        var transactions = await context.PropertyTransactions.ToListAsync();
        transactions.Should().HaveCount(1);
        transactions[0].Value.Should().Be(5000);
        transactions[0].CategoryId.Should().Be("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B6");
    }

    [Fact]
    public async Task ImportUserDataCommandValidator_ValidatesCommand()
    {
        var validator = new ImportUserDataCommandValidator();
        var command = new ImportUserDataCommand
        {
            UserId = UserId,
            FileContent = "some content"u8.ToArray()
        };
        var result = await validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.FileContent);
    }

    [Fact]
    public async Task ImportUserDataCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new ImportUserDataCommandValidator();
        var command = new ImportUserDataCommand { UserId = null, FileContent = [] };
        var result = await validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.FileContent);
    }
}