using FluentValidation;

namespace Fintrack.App.Functions.Property.Commands.RemoveProperty;

public sealed class RemovePropertyCommandValidator : AbstractValidator<RemovePropertyCommand>
{
    public RemovePropertyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PropertyId).NotEmpty();
    }
}