using FluentValidation;

namespace Fintrack.App.Functions.Property.Commands.RemovePropertyTransaction;

public sealed class RemovePropertyTransactionCommandValidator : AbstractValidator<RemovePropertyTransactionCommand>
{
    public RemovePropertyTransactionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PropertyId).NotEmpty();
        RuleFor(x => x.TransactionId).NotEmpty();
    }
}