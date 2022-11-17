using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.DuplicateNotification;

public sealed class DuplicateNotificationCommandValidator : AbstractValidator<DuplicateNotificationCommand>
{
    public DuplicateNotificationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NotificationId).NotEmpty();
    }
}