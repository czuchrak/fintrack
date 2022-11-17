using FluentValidation;

namespace Fintrack.App.Functions.Property.Commands.UpdatePropertyTransaction;

public sealed class UpdatePropertyTransactionCommandValidator : AbstractValidator<UpdatePropertyTransactionCommand>
{
    public UpdatePropertyTransactionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
    }
}