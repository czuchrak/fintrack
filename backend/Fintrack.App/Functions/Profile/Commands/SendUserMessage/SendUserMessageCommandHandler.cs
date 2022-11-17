using Fintrack.App.Mails;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Profile.Commands.SendUserMessage;

public class SendUserMessageCommandHandler : IRequestHandler<SendUserMessageCommand, Unit>
{
    private readonly ILogger<SendUserMessageCommandHandler> _logger;
    private readonly IMailSender _mailSender;

    public SendUserMessageCommandHandler(IMailSender mailSender,
        ILogger<SendUserMessageCommandHandler> logger)
    {
        _mailSender = mailSender;
        _logger = logger;
    }

    public async Task<Unit> Handle(SendUserMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        await _mailSender.SendUserMessage(request.Model, cancellationToken);

        _logger.LogInformation($"User {userId![..10]} has sent contact e-mail");

        return Unit.Value;
    }
}