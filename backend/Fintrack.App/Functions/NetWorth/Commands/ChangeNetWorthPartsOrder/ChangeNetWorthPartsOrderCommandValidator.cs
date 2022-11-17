using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.ChangeNetWorthPartsOrder;

public sealed class ChangeNetWorthPartsOrderCommandValidator : AbstractValidator<ChangeNetWorthPartsOrderCommand>
{
    public ChangeNetWorthPartsOrderCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PartIds).NotEmpty();
    }
}