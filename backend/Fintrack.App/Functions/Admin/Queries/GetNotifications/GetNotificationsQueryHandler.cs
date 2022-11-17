using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Queries.GetNotifications;

public class GetNotificationsQueryHandler : AdminBaseHandler,
    IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationModel>>
{
    public GetNotificationsQueryHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<IEnumerable<NotificationModel>> Handle(GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);

        var notifications = await Context.Notifications.ToListAsync(cancellationToken);
        var userNotifications = await Context.UserNotifications.ToListAsync(cancellationToken);
        var usersCount = await Context.Users.CountAsync(cancellationToken);

        return notifications.Select(x => new NotificationModel
        {
            Id = x.Id,
            Type = x.Type,
            Message = x.Message,
            Url = x.Url,
            ValidFrom = x.ValidFrom,
            ValidUntil = x.ValidUntil,
            IsActive = x.IsActive,
            ReadCount = userNotifications.Count(y => y.NotificationId == x.Id),
            UsersCount = usersCount
        });
    }
}