//using SendGrid;
//using SendGrid.Helpers.Mail;

//namespace Blog.Services;

//public class EmailService
//{
//    private readonly IConfiguration _config;

//    public EmailService(IConfiguration config)
//    {
//        _config = config;
//    }

//    public async Task SendContactEmailAsync(string name, string email, string message)
//    {
//        var apiKey = _config["SENDGRID_API_KEY"] ?? _config["SendGrid:ApiKey"];
//        var client = new SendGridClient(apiKey);

//        var from = new EmailAddress("nursevinc90@gmail.com", "nursevinc.com");
//        var to = new EmailAddress("nursevinc90@gmail.com");
//        var subject = $"Yeni İletişim Mesajı — {name}";
//        var html = $"""
//            <h2>Yeni bir mesaj aldın!</h2>
//            <p><strong>Ad:</strong> {name}</p>
//            <p><strong>E-posta:</strong> {email}</p>
//            <p><strong>Mesaj:</strong></p>
//            <p>{message}</p>
//        """;

//        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", html);
//        await client.SendEmailAsync(msg);
//    }
//}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
        var mail = new MimeMessage();
        mail.From.Add(MailboxAddress.Parse("b3c42b001@smtp-brevo.com"));
        mail.To.Add(MailboxAddress.Parse("nursevinc90@gmail.com"));
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
        await smtp.ConnectAsync("smtp-relay.brevo.com", 587, SecureSocketOptions.StartTls);
        var password = _config["BREVO_PASSWORD"];
        await smtp.AuthenticateAsync("b3c42b001@smtp-brevo.com", password);
        await smtp.SendAsync(mail);
        await smtp.DisconnectAsync(true);
    }
}