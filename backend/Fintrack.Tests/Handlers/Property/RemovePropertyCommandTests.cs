using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Commands.RemoveProperty;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Property;

public class RemovePropertyCommandTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        context.Properties.Add(new Database.Entities.Property
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Name = "Test Property",
            IsActive = true,
            UserId = UserId
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task RemovePropertyCommandHandler_RemovesProperty()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new RemovePropertyCommandHandler(context);

        await handler.Handle(new RemovePropertyCommand
        {
            PropertyId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            UserId = UserId
        }, CancellationToken.None);

        var properties = await context.Properties.ToListAsync();
        properties.Should().HaveCount(0);
    }

    [Fact]
    public async Task RemovePropertyCommandHandler_ThrowsException_WhenPropertyDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new RemovePropertyCommandHandler(context);

        var act = async () => await handler.Handle(new RemovePropertyCommand
        {
            PropertyId = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
            UserId = UserId
        }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RemovePropertyCommandValidator_ValidatesFields()
    {
        var validator = new RemovePropertyCommandValidator();
        var result = await validator.TestValidateAsync(new RemovePropertyCommand
        {
            PropertyId = Guid.NewGuid(),
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.PropertyId);
    }

    [Fact]
    public async Task RemovePropertyCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new RemovePropertyCommandValidator();
        var result = await validator.TestValidateAsync(new RemovePropertyCommand
        {
            PropertyId = Guid.Empty,
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.PropertyId);
    }
}