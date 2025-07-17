using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthPart;
using Fintrack.App.Functions.NetWorth.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.NetWorth;

public class UpdateNetWorthPartCommandTests : TestBase
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

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateNetWorthPartCommandHandler_UpdateNetWorthPart()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNetWorthPartCommandHandler(context);

        await handler.Handle(new UpdateNetWorthPartCommand
            {
                Model = new NetWorthPartModel
                {
                    Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                    Name = "Test123",
                    Type = "Liability",
                    IsVisible = false
                },
                UserId = UserId
            },
            CancellationToken.None);

        var parts = await context.NetWorthParts.ToListAsync();

        parts.Should().HaveCount(1);
        parts[0].Id.ToString().Should().NotBeEmpty();
        parts[0].Name.Should().Be("Test123");
        parts[0].Type.Should().Be("Liability");
        parts[0].Currency.Should().Be("PLN");
        parts[0].IsVisible.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateNetWorthPartCommandHandler_ThrowsException_WhenNetWorthPartDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdateNetWorthPartCommandHandler(context);

        var act = async () => await handler.Handle(new UpdateNetWorthPartCommand
        {
            Model = new NetWorthPartModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0")
            },
            UserId = UserId
        }, new CancellationToken());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateNetWorthPartCommandValidator_ValidatesFields()
    {
        var validator = new UpdateNetWorthPartCommandValidator();
        var result = await validator.TestValidateAsync(new UpdateNetWorthPartCommand
        {
            Model = new NetWorthPartModel { Id = Guid.NewGuid(), Name = "Test", Type = "Test2", Currency = "PLN" },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Currency);
    }

    [Fact]
    public async Task UpdateNetWorthPartCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new UpdateNetWorthPartCommandValidator();
        var result = await validator.TestValidateAsync(new UpdateNetWorthPartCommand
            { Model = new NetWorthPartModel(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldHaveValidationErrorFor(x => x.Model.Type);
        result.ShouldHaveValidationErrorFor(x => x.Model.Currency);
    }
}