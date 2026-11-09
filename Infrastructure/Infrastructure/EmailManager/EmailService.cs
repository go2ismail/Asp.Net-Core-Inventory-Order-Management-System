using Application.Common.Services.EmailManager;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.EmailManager;


public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtpSettings)
    {
        _logger = logger;
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(_smtpSettings.Host)
            || string.IsNullOrWhiteSpace(_smtpSettings.UserName)
            || string.IsNullOrWhiteSpace(_smtpSettings.Password))
        {
            _logger.LogWarning("SMTP settings are incomplete, so the email was not sent.");
            return;
        }

        try
        {
            var fromAddress = string.IsNullOrWhiteSpace(_smtpSettings.FromAddress)
                ? _smtpSettings.UserName
                : _smtpSettings.FromAddress;
            var fromName = string.IsNullOrWhiteSpace(_smtpSettings.FromName)
                ? "noreply"
                : _smtpSettings.FromName;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, true);
                await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email.");
        }
    }
}
