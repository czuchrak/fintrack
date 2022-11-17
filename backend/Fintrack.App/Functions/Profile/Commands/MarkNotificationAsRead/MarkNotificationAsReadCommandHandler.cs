using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Profile.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Unit>
{
    private readonly DatabaseContext _context;

    public MarkNotificationAsReadCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var notificationId = request.NotificationId;

        if (await _context.UserNotifications.AnyAsync(x =>
                x.NotificationId == notificationId && x.UserId == userId, cancellationToken))
            return Unit.Value;

        _context.UserNotifications.Add(new UserNotification
        {
            NotificationId = notificationId,
            UserId = userId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}