using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetExchangeRates;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class GetExchangeRatesQueryTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
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

    [Test]
    public async Task GetExchangeRatesQueryHandler_ReturnsAllExchangeRates()
    {
        await using var context = CreateContext();
        var handler = new GetExchangeRatesQueryHandler(context);

        var exchangeRates = (await handler.Handle(new GetExchangeRatesQuery { UserId = UserId }, new CancellationToken())).ToList();

        Assert.AreEqual(1, exchangeRates.Count);
        Assert.AreEqual("PLN", exchangeRates[0].Currency);
        Assert.AreEqual(5.1234M, exchangeRates[0].Rate);
        Assert.IsNotNull(exchangeRates[0].Date);
    }

    [Test]
    public async Task GetExchangeRatesQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new GetExchangeRatesQueryHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new GetExchangeRatesQuery { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task GetExchangeRatesQueryValidator_ValidatesUserId()
    {
        var validator = new GetExchangeRatesQueryValidator();
        var result = await validator.TestValidateAsync(new GetExchangeRatesQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task GetExchangeRatesQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetExchangeRatesQueryValidator();
        var result = await validator.TestValidateAsync(new GetExchangeRatesQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}