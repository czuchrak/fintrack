using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.UpdateNotification;

public class UpdateNotificationCommand : RequestBase, IRequest<Unit>
{
    public NotificationModel Model { get; set; }
}