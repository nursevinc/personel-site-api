using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Blog.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendContactEmailAsync(string name, string email, string message)
    {
        var settings = _config.GetSection("MailSettings");

        var mail = new MimeMessage();
        mail.From.Add(MailboxAddress.Parse(settings["From"]));
        mail.To.Add(MailboxAddress.Parse(settings["To"]));
        mail.Subject = $"Yeni İletişim Mesajı — {name}";

        mail.Body = new TextPart("html")
        {
            Text = $"""
                <h2>Yeni bir mesaj aldın!</h2>
                <p><strong>Ad:</strong> {name}</p>
                <p><strong>E-posta:</strong> {email}</p>
                <p><strong>Mesaj:</strong></p>
                <p>{message}</p>
            """
        };

        using var smtp = new SmtpClient();
        smtp.Timeout = 10000;
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("nursevinc90@gmail.com", "bvngmkowwbogiuln");
        await smtp.SendAsync(mail);
        await smtp.DisconnectAsync(true);
    }
}