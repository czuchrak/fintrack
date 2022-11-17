using FluentValidation;

namespace Fintrack.App.Functions.Property.Queries.GetProperties;

public sealed class GetPropertiesQueryValidator : AbstractValidator<GetPropertiesQuery>
{
    public GetPropertiesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}