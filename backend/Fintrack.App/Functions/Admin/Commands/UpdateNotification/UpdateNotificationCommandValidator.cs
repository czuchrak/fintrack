using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.UpdateNotification;

public sealed class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommand>
{
    public UpdateNotificationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
        RuleFor(x => x.Model.Type).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Model.Message).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Model.Url).MaximumLength(100);
    }
}