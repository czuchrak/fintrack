using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthEntry;

public sealed class RemoveNetWorthEntryCommandValidator : AbstractValidator<RemoveNetWorthEntryCommand>
{
    public RemoveNetWorthEntryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.EntryId).NotEmpty();
    }
}