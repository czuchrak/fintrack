using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetExchangeRates;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class GetExchangeRatesQueryTests : TestBase
{
    private async Task InitializeAsync()
    {
        await using var context = CreateContext();
        context.ExchangeRates.Add(new ExchangeRate
        {
            Currency = "PLN",
            Date = DateTime.Now,
            Rate = 5.1234M
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetExchangeRatesQueryHandler_ReturnsAllExchangeRates()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetExchangeRatesQueryHandler(context);

        var exchangeRates =
            (await handler.Handle(new GetExchangeRatesQuery { UserId = UserId }, CancellationToken.None)).ToList();

        exchangeRates.Should().HaveCount(1);
        exchangeRates[0].Currency.Should().Be("PLN");
        exchangeRates[0].Rate.Should().Be(5.1234M);
        exchangeRates[0].Date.Should().NotBe(default);
    }

    [Fact]
    public async Task GetExchangeRatesQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetExchangeRatesQueryHandler(context);

        var act = async () =>
            await handler.Handle(new GetExchangeRatesQuery { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetExchangeRatesQueryValidator_ValidatesUserId()
    {
        var validator = new GetExchangeRatesQueryValidator();
        var result = await validator.TestValidateAsync(new GetExchangeRatesQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetExchangeRatesQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetExchangeRatesQueryValidator();
        var result = await validator.TestValidateAsync(new GetExchangeRatesQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}