using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Queries.GetNotifications;

public class GetNotificationsQuery : RequestBase, IRequest<IEnumerable<NotificationModel>>
{
}