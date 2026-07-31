using Resend;

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
        var apiKey = _config["RESEND_API_KEY"];
        var resend = ResendClient.Create(apiKey);

        var msg = new EmailMessage
        {
            From = "info@nursevinc.com",
            To = { "nursevinc90@gmail.com" },
            Subject = $"Yeni İletişim Mesajı — {name}",
            HtmlBody = $"""
                <h2>Yeni bir mesaj aldın!</h2>
                <p><strong>Ad:</strong> {name}</p>
                <p><strong>E-posta:</strong> {email}</p>
                <p><strong>Mesaj:</strong></p>
                <p>{message}</p>
            """
        };

        await resend.EmailSendAsync(msg);
    }
}