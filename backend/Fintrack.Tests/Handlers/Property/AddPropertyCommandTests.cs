using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Commands.AddProperty;
using Fintrack.App.Functions.Property.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Property;

public class AddPropertyCommandTests : TestBase
{
    [Fact]
    public async Task AddPropertyCommandHandler_AddNewProperty()
    {
        await using var context = CreateContext();
        var handler = new AddPropertyCommandHandler(context);

        await handler.Handle(new AddPropertyCommand
        {
            Model = new PropertyModel
            {
                Name = "Test Property",
                IsActive = true
            },
            UserId = UserId
        }, CancellationToken.None);

        var properties = await context.Properties.ToListAsync();

        properties.Should().HaveCount(1);
        properties[0].Name.Should().Be("Test Property");
        properties[0].IsActive.Should().BeTrue();
        properties[0].UserId.Should().Be(UserId);
        properties[0].Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task AddPropertyCommandValidator_ValidatesFields()
    {
        var validator = new AddPropertyCommandValidator();
        var result = await validator.TestValidateAsync(new AddPropertyCommand
        {
            Model = new PropertyModel { Name = "Test Property", IsActive = true },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
    }

    [Fact]
    public async Task AddPropertyCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new AddPropertyCommandValidator();
        var result = await validator.TestValidateAsync(new AddPropertyCommand
        {
            Model = new PropertyModel(),
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
    }
}