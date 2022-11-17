using FluentValidation;

namespace Fintrack.App.Functions.Admin.Queries.GetExchangeRates;

public sealed class GetExchangeRatesQueryValidator : AbstractValidator<GetExchangeRatesQuery>
{
    public GetExchangeRatesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}