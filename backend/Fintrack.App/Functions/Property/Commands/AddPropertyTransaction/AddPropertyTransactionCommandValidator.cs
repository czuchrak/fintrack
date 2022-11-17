using FluentValidation;

namespace Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;

public sealed class AddPropertyTransactionCommandValidator : AbstractValidator<AddPropertyTransactionCommand>
{
    public AddPropertyTransactionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}