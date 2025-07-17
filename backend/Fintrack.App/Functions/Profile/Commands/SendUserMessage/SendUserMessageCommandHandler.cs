using Fintrack.App.Mails;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Profile.Commands.SendUserMessage;

public class SendUserMessageCommandHandler(
    IMailSender mailSender,
    ILogger<SendUserMessageCommandHandler> logger)
    : IRequestHandler<SendUserMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendUserMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        await mailSender.SendUserMessage(request.Model, cancellationToken);

        logger.LogInformation($"User {userId![..10]} has sent contact e-mail");

        return Unit.Value;
    }
}