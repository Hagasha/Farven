using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Farven Services", _configuration["MailSettings:Username"]));
        email.To.Add(new MailboxAddress("", to));
        email.Subject = subject;
        email.Body = new TextPart("html")
        {
            Text = body
        };

        using (var smtp = new SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;  // Apenas para desenvolvimento
            await smtp.ConnectAsync(_configuration["MailSettings:SmtpServer"],
                                    int.Parse(_configuration["MailSettings:Port"]),
                                    MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}