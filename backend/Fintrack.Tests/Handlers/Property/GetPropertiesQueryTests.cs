using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Queries.GetProperties;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Property;

public class GetPropertiesQueryTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        context.Properties.AddRange(
            new Database.Entities.Property
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B4"),
                Name = "Active Property",
                IsActive = true,
                UserId = UserId
            },
            new Database.Entities.Property
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B5"),
                Name = "Inactive Property",
                IsActive = false,
                UserId = UserId
            },
            new Database.Entities.Property
            {
                Id = new Guid("92EA3A0F-EBB8-43CE-AF8F-F5A8807484B6"),
                Name = "Other User Property",
                IsActive = true,
                UserId = "other-user"
            }
        );

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetPropertiesQueryHandler_ReturnsUserProperties()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetPropertiesQueryHandler(context);

        var result = await handler.Handle(new GetPropertiesQuery { UserId = UserId }, CancellationToken.None);

        var properties = result.ToList();
        properties.Should().HaveCount(2);
        properties.Should().Contain(p => p.Name == "Active Property" && p.IsActive == true);
        properties.Should().Contain(p => p.Name == "Inactive Property" && p.IsActive == false);
        properties.Should().NotContain(p => p.Name == "Other User Property");
    }

    [Fact]
    public async Task GetPropertiesQueryHandler_ReturnsEmptyForNewUser()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetPropertiesQueryHandler(context);

        var result = await handler.Handle(new GetPropertiesQuery { UserId = "new-user" }, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPropertiesQueryValidator_ValidatesUserId()
    {
        var validator = new GetPropertiesQueryValidator();
        var result = await validator.TestValidateAsync(new GetPropertiesQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetPropertiesQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetPropertiesQueryValidator();
        var result = await validator.TestValidateAsync(new GetPropertiesQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}