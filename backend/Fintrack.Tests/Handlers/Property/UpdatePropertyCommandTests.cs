using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Commands.UpdateProperty;
using Fintrack.App.Functions.Property.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Property;

public class UpdatePropertyCommandTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        context.Properties.Add(new Database.Entities.Property
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Name = "Original Property",
            IsActive = true,
            UserId = UserId
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdatePropertyCommandHandler_UpdatesProperty()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdatePropertyCommandHandler(context);

        await handler.Handle(new UpdatePropertyCommand
        {
            Model = new PropertyModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                Name = "Updated Property",
                IsActive = false
            },
            UserId = UserId
        }, CancellationToken.None);

        var property =
            await context.Properties.FirstAsync(x => x.Id == new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"));

        property.Name.Should().Be("Updated Property");
        property.IsActive.Should().BeFalse();
        property.UserId.Should().Be(UserId);
    }

    [Fact]
    public async Task UpdatePropertyCommandHandler_ThrowsException_WhenPropertyDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdatePropertyCommandHandler(context);

        var act = async () => await handler.Handle(new UpdatePropertyCommand
        {
            Model = new PropertyModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0"),
                Name = "Non-existent Property"
            },
            UserId = UserId
        }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdatePropertyCommandValidator_ValidatesFields()
    {
        var validator = new UpdatePropertyCommandValidator();
        var result = await validator.TestValidateAsync(new UpdatePropertyCommand
        {
            Model = new PropertyModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Property",
                IsActive = true
            },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
    }

    [Fact]
    public async Task UpdatePropertyCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new UpdatePropertyCommandValidator();
        var result = await validator.TestValidateAsync(new UpdatePropertyCommand
        {
            Model = new PropertyModel(),
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
    }
}