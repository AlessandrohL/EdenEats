using EdenEats.Application.DTOs.Identity;
using EdenEats.Application.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Email
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(EmailConfirmationInfo confirmationInfo);
        Task SendEmailAsync(Message message);
    }
}
