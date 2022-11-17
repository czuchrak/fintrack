using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthEntry;

public sealed class UpdateNetWorthEntryCommandValidator : AbstractValidator<UpdateNetWorthEntryCommand>
{
    public UpdateNetWorthEntryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
    }
}