using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.UpdatePropertyCategory;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class UpdatePropertyCategoryCommandTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();
        context.PropertyCategories.Add(new PropertyCategory
        {
            Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
            Type = "Test",
            Name = "Test2",
            IsCost = false
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdatePropertyCategoryCommandHandler_UpdatePropertyCategory()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdatePropertyCategoryCommandHandler(context);

        await handler.Handle(new UpdatePropertyCategoryCommand
            {
                Model = new PropertyCategoryModel
                {
                    Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                    Name = "Test23",
                    Type = "Test11",
                    IsCost = true
                },
                UserId = UserId
            },
            CancellationToken.None);

        var categories = await context.PropertyCategories.ToListAsync();

        categories.Should().HaveCount(1);
        categories[0].Id.ToString().Should().NotBeEmpty();
        categories[0].Name.Should().Be("Test23");
        categories[0].Type.Should().Be("Test11");
        categories[0].IsCost.Should().BeTrue();
    }

    [Fact]
    public async Task UpdatePropertyCategoryCommandHandler_ThrowsException_WhenPropertyCategoryDoesNotExist()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdatePropertyCategoryCommandHandler(context);

        var act = async () => await handler.Handle(new UpdatePropertyCategoryCommand
        {
            Model = new PropertyCategoryModel
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0")
            },
            UserId = UserId
        }, new CancellationToken());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdatePropertyCategoryCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new UpdatePropertyCategoryCommandHandler(context);

        var act = async () =>
            await handler.Handle(new UpdatePropertyCategoryCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task UpdatePropertyCategoryCommandValidator_ValidatesFields()
    {
        var validator = new UpdatePropertyCategoryCommandValidator();
        var result = await validator.TestValidateAsync(new UpdatePropertyCategoryCommand
        {
            Model = new PropertyCategoryModel { Id = Guid.NewGuid(), Name = "Test", Type = "Test2" },
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
    }

    [Fact]
    public async Task UpdatePropertyCategoryCommandValidator_ThrowsException_WhenFieldsAreIncorrect()
    {
        var validator = new UpdatePropertyCategoryCommandValidator();
        var result = await validator.TestValidateAsync(new UpdatePropertyCategoryCommand
            { Model = new PropertyCategoryModel(), UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Model.Id);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldHaveValidationErrorFor(x => x.Model.Type);
    }
}