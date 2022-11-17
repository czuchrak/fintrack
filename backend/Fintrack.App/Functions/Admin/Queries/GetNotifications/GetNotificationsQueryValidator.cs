using FluentValidation;

namespace Fintrack.App.Functions.Admin.Queries.GetNotifications;

public sealed class GetNotificationsQueryValidator : AbstractValidator<GetNotificationsQuery>
{
    public GetNotificationsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}