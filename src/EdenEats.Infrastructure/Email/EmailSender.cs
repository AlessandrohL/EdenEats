using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Email
{
    public sealed class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var mimeMessage = CreateEmailMessage(message);
            await SendAsync(mimeMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var mimeMessage = new MimeMessage();
            var addressList = message.To.Select(t => MailboxAddress.Parse(t));

            mimeMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            mimeMessage.To.AddRange(addressList);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message.Content };

            return mimeMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();

            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await client.SendAsync(mailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
