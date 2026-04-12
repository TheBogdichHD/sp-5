using System.Net;
using System.Net.Mail;
using Lab5.Areas.Communication.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lab5.Areas.Communication.Services;

public class GmailEmailNotificationService : IEmailNotificationService
{
    private readonly GmailSmtpOptions _options;
    private readonly ILogger<GmailEmailNotificationService> _logger;

    public GmailEmailNotificationService(
        IOptions<GmailSmtpOptions> options,
        ILogger<GmailEmailNotificationService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task SendSubscriptionAsync(string subscriberEmail)
    {
        var subject = "New Subscribe Request";
        var body = $"New subscription email: {subscriberEmail}";
        return SendAsync(subscriberEmail, subject, body);
    }

    public Task SendContactAsync(ContactRequest request)
    {
        var subject = $"New Contact Message: {request.Subject}";
        var body =
            $"Name: {request.Name}{Environment.NewLine}" +
            $"Email: {request.Email}{Environment.NewLine}" +
            $"Subject: {request.Subject}{Environment.NewLine}" +
            $"Message:{Environment.NewLine}{request.Message}";

        return SendAsync(_options.ToEmail, subject, body);
    }

    private async Task SendAsync(string toEmail, string subject, string body)
    {
        ValidateOptions(toEmail);

        _logger.LogInformation("Sending email to {ToEmail} via {Host}:{Port}", toEmail, _options.Host, _options.Port);

        using var message = new MailMessage(_options.FromEmail, toEmail)
        {
            Subject = subject,
            Body = body
        };

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_options.Username, _options.AppPassword),
            Timeout = 30000
        };

        try
        {
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
            throw;
        }
    }

    private void ValidateOptions(string toEmail)
    {
        if (string.IsNullOrWhiteSpace(_options.Username) ||
            string.IsNullOrWhiteSpace(_options.AppPassword) ||
            string.IsNullOrWhiteSpace(_options.FromEmail) ||
            string.IsNullOrWhiteSpace(toEmail))
        {
            throw new InvalidOperationException(
                "Gmail SMTP settings are not configured. Please set the following environment variables:\n" +
                "GMAIL_SMTP_USERNAME, GMAIL_SMTP_APP_PASSWORD, GMAIL_SMTP_FROM_EMAIL, GMAIL_SMTP_TO_EMAIL");
        }
    }
}
