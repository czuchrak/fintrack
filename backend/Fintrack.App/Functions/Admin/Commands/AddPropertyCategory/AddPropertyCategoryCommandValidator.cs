using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.AddPropertyCategory;

public sealed class AddPropertyCategoryCommandValidator : AbstractValidator<AddPropertyCategoryCommand>
{
    public AddPropertyCategoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Model.Type).NotEmpty().MaximumLength(20);
    }
}