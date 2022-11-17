using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Queries.GetNetWorthModel;

public sealed class GetNetWorthModelQueryValidator : AbstractValidator<GetNetWorthModelQuery>
{
    public GetNetWorthModelQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}