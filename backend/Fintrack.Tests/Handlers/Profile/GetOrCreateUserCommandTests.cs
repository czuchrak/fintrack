using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Profile.Commands.GetOrCreateUser;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Profile;

public class GetOrCreateUserCommandTests : TestBase
{
    [Fact]
    public async Task GetOrCreateUserCommandHandler_CreatesNewUser()
    {
        await using var context = CreateContext();
        var handler = new GetOrCreateUserCommandHandler(context);

        var result = await handler.Handle(new GetOrCreateUserCommand
        {
            UserId = "new-user-id",
            Email = "test@example.com"
        }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Currencies.Should().HaveCountGreaterThan(0);
        result.Properties.Should().NotBeNull();
        result.UserSettings.Should().NotBeNull();

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == "new-user-id");
        user.Should().NotBeNull();
        user!.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetOrCreateUserCommandHandler_ReturnsExistingUser()
    {
        await using var context = CreateContext();

        // Create existing user
        context.Users.Add(new User
        {
            Id = "existing-user-id",
            Email = "existing@example.com",
            Currency = "PLN",
            NewMonthEmailEnabled = true,
            NewsEmailEnabled = false
        });
        await context.SaveChangesAsync();

        var handler = new GetOrCreateUserCommandHandler(context);

        var result = await handler.Handle(new GetOrCreateUserCommand
        {
            UserId = "existing-user-id",
            Email = "existing@example.com"
        }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Currency.Should().Be("PLN");
        result.UserSettings.NewMonthEmailEnabled.Should().BeTrue();
        result.UserSettings.NewsEmailEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task GetOrCreateUserCommandValidator_ValidatesFields()
    {
        var validator = new GetOrCreateUserCommandValidator();
        var result = await validator.TestValidateAsync(new GetOrCreateUserCommand
        {
            UserId = "test-user",
            Email = "test@example.com"
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}