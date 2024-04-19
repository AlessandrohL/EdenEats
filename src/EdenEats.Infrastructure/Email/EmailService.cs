using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Contracts.Utilities;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Application.Email;
using EdenEats.Infrastructure.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Email
{
    public sealed class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ClientConfiguration _clientConfig;

        public EmailService(
            IEmailSender emailSender,
            ClientConfiguration clientConfig)
        {
            _emailSender = emailSender;
            _clientConfig = clientConfig;
        }

        public async Task SendEmailAsync(Message message)
            => await _emailSender.SendEmailAsync(message);

        public async Task SendEmailConfirmationAsync(UserIdentityDTO userIdentity, string confirmationToken, string names)
        {
            if (userIdentity == null)
            {
                throw new ArgumentNullException(nameof(userIdentity));
            }

            if (string.IsNullOrEmpty(confirmationToken))
            {
                throw new ArgumentException($"{nameof(confirmationToken)} cannot be null or empty.");
            }

            var confirmationUrl = GenerateEmailConfirmationUrl(userIdentity.Id, confirmationToken);
            var body = CreateEmailConfirmationTemplate(names, confirmationUrl);
            var emailConfirmationMessage = new Message(
                to: new string[1] { userIdentity.Email },
                subject: EmailConstants.SubjectConfirmationEmail,
                content: body);

            await SendEmailAsync(emailConfirmationMessage);
        }

        private static string CreateEmailConfirmationTemplate(string firstNames, string confirmationUrl)
        {
            string templatePath = GetEmailComfirmationTemplatePath();

            string templateContent = File.ReadAllText(templatePath);
            templateContent = templateContent.Replace("{userFirstName}", firstNames);
            templateContent = templateContent.Replace("{confirmationLink}", confirmationUrl);

            return templateContent;
        }

        private string GenerateEmailConfirmationUrl(Guid identityId, string confirmationToken)
        {
            var clientConfirmationUrl = $"{_clientConfig.BaseUrl}/{_clientConfig.ConfirmationEndpoint}";
            var confirmationLink = $"{clientConfirmationUrl}?id={identityId}&code={confirmationToken}";

            return confirmationLink;
        }

        private static string GetEmailComfirmationTemplatePath()
        {
            var dirPath = Directory.GetCurrentDirectory() + "\\Templates\\";
            var templatePath = dirPath + "EmailConfirmation.html";

            return templatePath;
        }
    }
}
