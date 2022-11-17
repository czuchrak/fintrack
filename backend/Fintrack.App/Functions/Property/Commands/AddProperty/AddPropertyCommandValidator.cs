using FluentValidation;

namespace Fintrack.App.Functions.Property.Commands.AddProperty;

public sealed class AddPropertyCommandValidator : AbstractValidator<AddPropertyCommand>
{
    public AddPropertyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(40);
    }
}