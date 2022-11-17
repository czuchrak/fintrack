using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler : AdminBaseHandler, IRequestHandler<DeleteNotificationCommand, Unit>
{
    public DeleteNotificationCommandHandler(DatabaseContext context) : base(context)
    {
    }

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