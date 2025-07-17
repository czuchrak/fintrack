using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.AddPropertyCategory;
using Fintrack.App.Functions.Admin.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class AddPropertyCategoryCommandTests : TestBase
{
    [Fact]
    public async Task AddPropertyCategoryCommandHandler_AddNewPropertyCategory()
    {
        await using var context = CreateContext();
        var handler = new AddPropertyCategoryCommandHandler(context);

        await handler.Handle(new AddPropertyCategoryCommand
            {
                Model = new PropertyCategoryModel
                {
                    Name = Guid.NewGuid().ToString(),
                    Type = Guid.NewGuid().ToString(),
                    IsCost = true
                },
                UserId = UserId
            },
            CancellationToken.None);

        var categories = await context.PropertyCategories.ToListAsync();

        categories.Should().HaveCount(1);
        categories[0].Id.ToString().Should().NotBeEmpty();
        categories[0].Name.Should().NotBeEmpty();
        categories[0].Type.Should().NotBeEmpty();
        categories[0].IsCost.Should().BeTrue();
    }

    [Fact]
    public async Task AddPropertyCategoryCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new AddPropertyCategoryCommandHandler(context);

        var act = async () =>
            await handler.Handle(new AddPropertyCategoryCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task AddPropertyCategoryCommandValidator_ValidatesFields()
    {
        var validator = new AddPropertyCategoryCommandValidator();
        var result = await validator.TestValidateAsync(new AddPropertyCategoryCommand
            { Model = new PropertyCategoryModel { Name = "Test", Type = "Test2" }, UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
    }

    [Fact]
    public async Task AddPropertyCategoryCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new AddPropertyCategoryCommandValidator();
        var result = await validator.TestValidateAsync(new AddPropertyCategoryCommand
            { Model = new PropertyCategoryModel(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldHaveValidationErrorFor(x => x.Model.Type);
    }
}