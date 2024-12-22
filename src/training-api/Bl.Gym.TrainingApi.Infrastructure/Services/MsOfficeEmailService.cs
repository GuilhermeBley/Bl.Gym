using Bl.Gym.TrainingApi.Application.Services;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using System.Net;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Bl.Gym.TrainingApi.Infrastructure.Options;
using System.Runtime.InteropServices.JavaScript;

namespace Bl.Gym.TrainingApi.Infrastructure.Services;

public class MsOfficeEmailService
    : IEmailService
{
    private const int SMTP_PORT = 587;

    private readonly NetworkCredential _credential;
    private readonly string _host;
    private readonly MailboxAddress _defaultFromMailAddress;

    public MsOfficeEmailService(IOptions<EmailOption> options)
    {
        var optionsValue = options.Value;

        _credential = new NetworkCredential(optionsValue.UserName, optionsValue.Password);
        _host = optionsValue.Host;

        _defaultFromMailAddress = new(optionsValue.AddressNameFrom, optionsValue.AddressFrom);
    }

    public async Task SendGymInvitationEmailAsync(
        UserGymInvitation invite, 
        Uri redirectUri, 
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var mimeMessage = new MimeMessage();

        var bodyHtml = """
                        <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Welcome to the gym</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f4f4f4;
                        color: #333333;
                    }
                    .email-container {
                        max-width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        border-radius: 10px;
                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                        overflow: hidden;
                    }
                    .email-header {
                        background-color: #4CAF50;
                        color: #ffffff;
                        padding: 20px;
                        text-align: center;
                    }
                    .email-header h1 {
                        margin: 0;
                        font-size: 24px;
                    }
                    .email-body {
                        padding: 20px;
                        text-align: left;
                    }
                    .email-body p {
                        font-size: 16px;
                        line-height: 1.6;
                        margin: 10px 0;
                    }
                    .email-footer {
                        background-color: #f4f4f4;
                        padding: 10px;
                        text-align: center;
                        font-size: 14px;
                        color: #777777;
                    }
                    .btn {
                        display: inline-block;
                        background-color: #4CAF50;
                        color: #ffffff;
                        text-decoration: none;
                        padding: 10px 20px;
                        border-radius: 5px;
                        font-size: 16px;
                        font-weight: bold;
                        margin-top: 20px;
                    }
                    .btn:hover {
                        background-color: #45a049;
                    }
                </style>
            </head>
            <body>
                <div class="email-container">
                    <div class="email-header">
                        <h1>Welcome!</h1>
                    </div>
                    <div class="email-body">
                        <p>Hello,</p>

                        <p>We’re thrilled to invite you to join our fitness community! At [Gym Name], we are committed to helping you achieve your fitness goals and lead a healthier, happier life.</p>

                        <p>Click the button below to accept your invitation and explore everything we have to offer:</p>

                        <p style="text-align: center;">
                            <a href="{url}" class="btn">Accept Your Invite</a>
                        </p>

                        <p>Feel free to reach out to us with any questions or to schedule a tour of our facilities. We can’t wait to see you!</p>

                        <p>Best regards</p>
                    </div>
                    <div class="email-footer">
                        <p>&copy; {year}. All rights reserved.</p>
                        <p>123 Fitness Lane, Workout City, FitState 45678</p>
                    </div>
                </div>
            </body>
            </html>
            
            """
            .Replace("{url}", redirectUri.AbsoluteUri)
            .Replace("{year}", DateTime.UtcNow.Year.ToString());

        mimeMessage.From.Add(_defaultFromMailAddress);

        mimeMessage.To.Add(
            new MailboxAddress("", invite.UserEmail));

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_host, SMTP_PORT, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_credential, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await client.SendAsync(mimeMessage, cancellationToken);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
