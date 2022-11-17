using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.AddPropertyCategory;
using Fintrack.App.Functions.Admin.Models;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class AddPropertyCategoryCommandTests : TestBase
{
    [Test]
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
            new CancellationToken());

        var categories = await context.PropertyCategories.ToListAsync();

        Assert.AreEqual(1, categories.Count);
        Assert.IsNotEmpty(categories[0].Id.ToString());
        Assert.IsNotEmpty(categories[0].Name);
        Assert.IsNotEmpty(categories[0].Type);
        Assert.AreEqual(true, categories[0].IsCost);
    }

    [Test]
    public async Task AddPropertyCategoryCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new AddPropertyCategoryCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new AddPropertyCategoryCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task AddPropertyCategoryCommandValidator_ValidatesFields()
    {
        var validator = new AddPropertyCategoryCommandValidator();
        var result = await validator.TestValidateAsync(new AddPropertyCategoryCommand
            { Model = new PropertyCategoryModel { Name = "Test", Type = "Test2" }, UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Type);
    }

    [Test]
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