using FluentValidation;

namespace Fintrack.App.Functions.Property.Commands.UpdateProperty;

public sealed class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(40);
    }
}