using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Email
{
    public record EmailConfirmationInfo(
        Guid UserId, 
        string Email, 
        string ConfirmationToken,
        string Names);
}
