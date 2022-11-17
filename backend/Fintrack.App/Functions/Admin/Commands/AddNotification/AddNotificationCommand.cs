using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.AddNotification;

public class AddNotificationCommand : RequestBase, IRequest
{
    public NotificationModel Model { get; set; }
}