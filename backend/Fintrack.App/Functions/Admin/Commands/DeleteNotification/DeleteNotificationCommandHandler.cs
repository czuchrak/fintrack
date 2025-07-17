using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler(DatabaseContext context)
    : AdminBaseHandler(context), IRequestHandler<DeleteNotificationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);
        var notificationId = request.NotificationId;

        var notification = await Context.Notifications
            .SingleAsync(x => x.Id == notificationId, cancellationToken);

        Context.Notifications.Remove(notification);

        await Context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}