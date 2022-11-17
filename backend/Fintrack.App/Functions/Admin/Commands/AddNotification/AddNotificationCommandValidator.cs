using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.AddNotification;

public sealed class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommand>
{
    public AddNotificationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Type).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Model.Message).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Model.Url).MaximumLength(100);
    }
}