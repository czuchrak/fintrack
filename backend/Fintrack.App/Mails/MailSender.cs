using Fintrack.App.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Fintrack.App.Mails;

public interface IMailSender
{
    Task SendUserMessage(MessageModel message, CancellationToken cancellationToken);
    Task SendStatusMessage(string message, CancellationToken cancellationToken);
}

public class MailSender : IMailSender
{
    private const string AppName = "Fintrack.app";
    private readonly MailSettings _mailSettings;

    public MailSender(IOptions<MailSettings> mailSettingsAccessor)
    {
        _mailSettings = mailSettingsAccessor.Value;
    }

    public async Task SendStatusMessage(string message, CancellationToken cancellationToken)
    {
        await SendMail(
            CreateMessage(_mailSettings.ContactMail, _mailSettings.ContactMail, "Status", message),
            cancellationToken);
    }

    public async Task SendUserMessage(MessageModel message, CancellationToken cancellationToken)
    {
        await SendMail(
            CreateMessage(_mailSettings.ContactMail, message.Email, message.Topic, message.Message),
            cancellationToken);
    }

    private MimeMessage CreateMessage(string emailTo, string replyTo, string subject, string message)
    {
        var mail = new MimeMessage();
        mail.From.Add(MailboxAddress.Parse(_mailSettings.ContactMail));
        mail.To.Add(new MailboxAddress(AppName, emailTo));
        mail.ReplyTo.Add(MailboxAddress.Parse(replyTo));
        mail.Subject = subject;

        mail.Body = new TextPart("html")
        {
            Text = message.Replace("\n", "<br/>")
        };

        return mail;
    }

    private async Task SendMail(MimeMessage mail, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, true, cancellationToken);
        await client.AuthenticateAsync(_mailSettings.ContactMail, _mailSettings.SmtpPassword, cancellationToken);
        await client.SendAsync(mail, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}

public class MailSettings
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpPassword { get; set; }
    public string ContactMail { get; set; }
}