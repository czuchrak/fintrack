using FluentValidation;

namespace Fintrack.App.Functions.Profile.Commands.SendUserMessage;

public sealed class SendUserMessageCommandValidator : AbstractValidator<SendUserMessageCommand>
{
    public SendUserMessageCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Email).NotEmpty();
        RuleFor(x => x.Model.Topic).NotEmpty();
        RuleFor(x => x.Model.Message).NotEmpty();
    }
}