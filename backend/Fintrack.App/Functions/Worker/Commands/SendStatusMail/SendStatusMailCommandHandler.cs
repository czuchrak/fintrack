using System.Text;
using Fintrack.App.Mails;
using Fintrack.Database;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Worker.Commands.SendStatusMail;

public class SendStatusMailCommandHandler : IRequestHandler<SendStatusMailCommand, Unit>
{
    private readonly DatabaseContext _context;
    private readonly IHostingEnvironment _env;
    private readonly ILogger<SendStatusMailCommandHandler> _logger;
    private readonly IMailSender _mailSender;

    public SendStatusMailCommandHandler(DatabaseContext context,
        ILogger<SendStatusMailCommandHandler> logger,
        IHostingEnvironment env,
        IMailSender mailSender)
    {
        _context = context;
        _logger = logger;
        _env = env;
        _mailSender = mailSender;
    }

    public async Task<Unit> Handle(SendStatusMailCommand request, CancellationToken cancellationToken)
    {
        if (!_env.IsProduction()) return Unit.Value;

        var message = new StringBuilder();
        await AppendRates(message, cancellationToken);
        await AppendLogs(message, cancellationToken);

        await _mailSender.SendStatusMessage(message.ToString(), cancellationToken);
        _logger.LogInformation("Status message has been sent");

        return Unit.Value;
    }

    private async Task AppendRates(StringBuilder message, CancellationToken cancellationToken)
    {
        var lastRates = await _context.ExchangeRates
            .OrderByDescending(x => x.Date)
            .Take(4)
            .ToListAsync(cancellationToken);

        message.AppendLine("<h3>Last rates:</h3>");

        foreach (var rate in lastRates)
            message.AppendLine($"{rate.Date.ToString("yyyy-MM-dd")}\t{rate.Currency}\t{Math.Round(rate.Rate, 4)}");
    }

    private async Task AppendLogs(StringBuilder message, CancellationToken cancellationToken)
    {
        var logs = await _context.Logs
            .Where(x => x.TimeStamp > DateTime.Now.AddDays(-1))
            .OrderByDescending(x => x.TimeStamp)
            .ToListAsync(cancellationToken);

        message.AppendLine("<h3>Logs:</h3>");

        foreach (var log in logs)
            message.AppendLine(
                $"{log.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")}\t{Truncate(log.Message)}\t{Truncate(log.Exception)}");
    }

    private static string Truncate(string value)
    {
        return value.Length > 100 ? value[..100] : value;
    }
}