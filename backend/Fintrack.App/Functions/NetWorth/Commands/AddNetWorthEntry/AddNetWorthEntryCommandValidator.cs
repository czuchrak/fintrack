using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthEntry;

public sealed class AddNetWorthEntryCommandValidator : AbstractValidator<AddNetWorthEntryCommand>
{
    public AddNetWorthEntryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Date).NotEmpty();
    }
}