using Klient.Management.Service.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Klient.Management.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _EmailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _EmailConfiguration = emailConfiguration;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _EmailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var builder = new BodyBuilder();
            builder.TextBody = string.Format (message.Content);
            builder.HtmlBody = string.Format (message.Html);
            emailMessage.Body = builder.ToMessageBody();
            
            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using var emailClient = new SmtpClient();
            try
            {
                emailClient.Connect(_EmailConfiguration.SmtpServer, _EmailConfiguration.Port, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_EmailConfiguration.Username, _EmailConfiguration.Password);

                emailClient.Send(message);
            }
            catch
            {
                throw;
            }
            finally
            {
                emailClient.Disconnect(true);
                emailClient.Dispose();
            }
        }
    }
}
