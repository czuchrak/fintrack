using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;
using Fintrack.App.Functions.Property.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Property;

public class AddPropertyTransactionCommandTests : TestBase
{
    [Fact]
    public async Task AddPropertyTransactionCommandHandler_AddNewTransaction()
    {
        await using var context = CreateContext();

        context.Properties.Add(new Database.Entities.Property
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Name = "Test Property",
            IsActive = true,
            UserId = UserId
        });

        context.PropertyCategories.Add(new PropertyCategory
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
            Name = "Test Category",
            Type = "Income",
            IsCost = false
        });

        await context.SaveChangesAsync();

        var handler = new AddPropertyTransactionCommandHandler(context);

        await handler.Handle(new AddPropertyTransactionCommand
        {
            Model = new PropertyTransactionModel
            {
                PropertyId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                CategoryId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
                Value = 1500M,
                Date = DateTime.Parse("2022-01-01"),
                Details = "Test Transaction"
            },
            UserId = UserId
        }, CancellationToken.None);

        var transactions = await context.PropertyTransactions.ToListAsync();

        transactions.Should().HaveCount(1);
        transactions[0].PropertyId.Should().Be(new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"));
        transactions[0].CategoryId.Should().Be(new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"));
        transactions[0].Value.Should().Be(1500M);
        transactions[0].Date.Should().Be(DateTime.Parse("2022-01-01"));
        transactions[0].Details.Should().Be("Test Transaction");
    }

    [Fact]
    public async Task AddPropertyTransactionCommandValidator_ValidatesFields()
    {
        var validator = new AddPropertyTransactionCommandValidator();
        var result = await validator.TestValidateAsync(new AddPropertyTransactionCommand
        {
            Model = new PropertyTransactionModel
            {
                PropertyId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                Value = 1000M,
                Date = DateTime.Now,
                Details = "Test"
            },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.PropertyId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.CategoryId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Value);
    }
}