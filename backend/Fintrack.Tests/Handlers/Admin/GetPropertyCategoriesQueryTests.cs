using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetPropertyCategories;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class GetPropertyCategoriesQueryTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        var id = Guid.NewGuid();
        await using var context = CreateContext();
        context.PropertyCategories.Add(new PropertyCategory
        {
            Id = id,
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            IsCost = true
        });

        context.PropertyTransactions.Add(new PropertyTransaction
        {
            CategoryId = id
        });

        await context.SaveChangesAsync();
    }

    [Test]
    public async Task GetPropertyCategoriesQueryHandler_ReturnsAllCategoryProperties()
    {
        await using var context = CreateContext();
        var handler = new GetPropertyCategoriesQueryHandler(context);

        var categories =
            (await handler.Handle(new GetPropertyCategoriesQuery { UserId = UserId }, new CancellationToken()))
            .ToList();

        Assert.AreEqual(1, categories.Count);
        Assert.IsNotEmpty(categories[0].Id.ToString());
        Assert.IsNotEmpty(categories[0].Name);
        Assert.IsNotEmpty(categories[0].Type);
        Assert.AreEqual(true, categories[0].IsCost);
        Assert.AreEqual(1, categories[0].Count);
    }

    [Test]
    public async Task GetPropertyCategoriesQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new GetPropertyCategoriesQueryHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new GetPropertyCategoriesQuery { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task GetPropertyCategoriesQueryValidator_ValidatesUserId()
    {
        var validator = new GetPropertyCategoriesQueryValidator();
        var result = await validator.TestValidateAsync(new GetPropertyCategoriesQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task GetPropertyCategoriesQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetPropertyCategoriesQueryValidator();
        var result = await validator.TestValidateAsync(new GetPropertyCategoriesQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}