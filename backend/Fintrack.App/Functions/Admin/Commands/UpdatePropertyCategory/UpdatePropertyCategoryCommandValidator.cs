using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.UpdatePropertyCategory;

public sealed class UpdatePropertyCategoryCommandValidator : AbstractValidator<UpdatePropertyCategoryCommand>
{
    public UpdatePropertyCategoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Model.Type).NotEmpty().MaximumLength(20);
    }
}