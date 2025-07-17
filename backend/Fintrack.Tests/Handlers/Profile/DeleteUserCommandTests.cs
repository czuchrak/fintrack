using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Profile.Commands.DeleteUser;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Profile;

public class DeleteUserCommandTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        context.NetWorthParts.Add(new NetWorthPart
        {
            Id = Guid.NewGuid(),
            Name = "Test Part",
            Type = "Asset",
            Currency = "PLN",
            IsVisible = true,
            UserId = UserId,
            Order = 1
        });

        context.Properties.Add(new Database.Entities.Property
        {
            Id = Guid.NewGuid(),
            Name = "Test Property",
            IsActive = true,
            UserId = UserId
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteUserCommandHandler_RemovesUserAndAllData()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DeleteUserCommandHandler(context);

        await handler.Handle(new DeleteUserCommand
        {
            UserId = UserId
        }, CancellationToken.None);

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        var parts = await context.NetWorthParts.Where(x => x.UserId == UserId).ToListAsync();
        var properties = await context.Properties.Where(x => x.UserId == UserId).ToListAsync();

        user.Should().BeNull();
        parts.Should().BeEmpty();
        properties.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteUserCommandValidator_ValidatesFields()
    {
        var validator = new DeleteUserCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteUserCommand
        {
            UserId = UserId
        });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task DeleteUserCommandValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new DeleteUserCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteUserCommand
        {
            UserId = null
        });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}