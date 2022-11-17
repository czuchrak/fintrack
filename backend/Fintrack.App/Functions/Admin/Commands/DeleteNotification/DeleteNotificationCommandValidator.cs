using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.DeleteNotification;

public sealed class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
{
    public DeleteNotificationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NotificationId).NotEmpty();
    }
}