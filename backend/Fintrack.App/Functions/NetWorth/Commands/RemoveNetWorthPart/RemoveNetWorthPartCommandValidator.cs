using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthPart;

public sealed class RemoveNetWorthPartCommandValidator : AbstractValidator<RemoveNetWorthPartCommand>
{
    public RemoveNetWorthPartCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PartId).NotEmpty();
    }
}