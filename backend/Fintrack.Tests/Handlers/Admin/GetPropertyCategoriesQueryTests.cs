using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetPropertyCategories;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class GetPropertyCategoriesQueryTests : TestBase
{
    private async Task InitializeAsync()
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

    [Fact]
    public async Task GetPropertyCategoriesQueryHandler_ReturnsAllCategoryProperties()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetPropertyCategoriesQueryHandler(context);

        var categories =
            (await handler.Handle(new GetPropertyCategoriesQuery { UserId = UserId }, CancellationToken.None))
            .ToList();

        categories.Should().HaveCount(1);
        categories[0].Id.ToString().Should().NotBeEmpty();
        categories[0].Name.Should().NotBeEmpty();
        categories[0].Type.Should().NotBeEmpty();
        categories[0].IsCost.Should().BeTrue();
        categories[0].Count.Should().Be(1);
    }

    [Fact]
    public async Task GetPropertyCategoriesQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetPropertyCategoriesQueryHandler(context);

        var act = async () =>
            await handler.Handle(new GetPropertyCategoriesQuery { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetPropertyCategoriesQueryValidator_ValidatesUserId()
    {
        var validator = new GetPropertyCategoriesQueryValidator();
        var result = await validator.TestValidateAsync(new GetPropertyCategoriesQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetPropertyCategoriesQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetPropertyCategoriesQueryValidator();
        var result = await validator.TestValidateAsync(new GetPropertyCategoriesQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}