using Fintrack.App.Models;
using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.SendUserMessage;

public class SendUserMessageCommand : RequestBase, IRequest
{
    public MessageModel Model { get; set; }
}