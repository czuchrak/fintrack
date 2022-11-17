using FluentValidation;

namespace Fintrack.App.Functions.Admin.Queries.GetPropertyCategories;

public sealed class GetPropertyCategoriesQueryValidator : AbstractValidator<GetPropertyCategoriesQuery>
{
    public GetPropertyCategoriesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}