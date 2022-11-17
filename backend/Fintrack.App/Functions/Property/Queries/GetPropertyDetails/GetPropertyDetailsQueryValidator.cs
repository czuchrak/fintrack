using FluentValidation;

namespace Fintrack.App.Functions.Property.Queries.GetPropertyDetails;

public sealed class GetPropertyDetailsQueryValidator : AbstractValidator<GetPropertyDetailsQuery>
{
    public GetPropertyDetailsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PropertyId).NotEmpty();
    }
}