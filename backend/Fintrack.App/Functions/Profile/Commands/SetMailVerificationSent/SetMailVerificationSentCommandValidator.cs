using FluentValidation;

namespace Fintrack.App.Functions.Profile.Commands.SetMailVerificationSent;

public sealed class SetMailVerificationSentCommandValidator : AbstractValidator<SetMailVerificationSentCommand>
{
    public SetMailVerificationSentCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}