using EdenEats.Application.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
