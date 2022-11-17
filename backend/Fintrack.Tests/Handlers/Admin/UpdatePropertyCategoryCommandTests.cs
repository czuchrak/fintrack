using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.UpdatePropertyCategory;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class UpdatePropertyCategoryCommandTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
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

    [Test]
    public async Task UpdatePropertyCategoryCommandHandler_UpdatePropertyCategory()
    {
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
            new CancellationToken());

        var categories = await context.PropertyCategories.ToListAsync();

        Assert.AreEqual(1, categories.Count);
        Assert.IsNotEmpty(categories[0].Id.ToString());
        Assert.AreEqual("Test23", categories[0].Name);
        Assert.AreEqual("Test11", categories[0].Type);
        Assert.AreEqual(true, categories[0].IsCost);
    }

    [Test]
    public async Task UpdatePropertyCategoryCommandHandler_ThrowsException_WhenPropertyCategoryDoesNotExist()
    {
        await using var context = CreateContext();
        var handler = new UpdatePropertyCategoryCommandHandler(context);

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await handler.Handle(new UpdatePropertyCategoryCommand
            {
                Model = new PropertyCategoryModel
                {
                    Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B0")
                },
                UserId = UserId
            }, new CancellationToken()));
    }

    [Test]
    public async Task UpdatePropertyCategoryCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new UpdatePropertyCategoryCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new UpdatePropertyCategoryCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
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

    [Test]
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