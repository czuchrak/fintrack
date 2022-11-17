using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Commands.DuplicateNotification;

public class DuplicateNotificationCommandHandler : AdminBaseHandler, IRequestHandler<DuplicateNotificationCommand, Unit>
{
    public DuplicateNotificationCommandHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<Unit> Handle(DuplicateNotificationCommand request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);
        var notificationId = request.NotificationId;

        var notification = await Context.Notifications
            .SingleAsync(x => x.Id == notificationId, cancellationToken);

        Context.Notifications.Add(new Notification
        {
            Type = notification.Type,
            Message = notification.Message,
            Url = notification.Url,
            ValidFrom = notification.ValidFrom,
            ValidUntil = notification.ValidUntil,
            IsActive = false
        });

        await Context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}