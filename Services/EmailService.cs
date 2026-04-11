using SendGrid;
using SendGrid.Helpers.Mail;

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
        var apiKey = _config["SENDGRID_API_KEY"] ?? _config["SendGrid:ApiKey"];
        var client = new SendGridClient(apiKey);

        var from = new EmailAddress("nursevinc90@gmail.com", "nursevinc.com");
        var to = new EmailAddress("nursevinc90@gmail.com");
        var subject = $"Yeni İletişim Mesajı — {name}";
        var html = $"""
            <h2>Yeni bir mesaj aldın!</h2>
            <p><strong>Ad:</strong> {name}</p>
            <p><strong>E-posta:</strong> {email}</p>
            <p><strong>Mesaj:</strong></p>
            <p>{message}</p>
        """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", html);
        await client.SendEmailAsync(msg);
    }
}