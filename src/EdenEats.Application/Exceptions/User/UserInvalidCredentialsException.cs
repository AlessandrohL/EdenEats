using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.User
{
    public sealed class UserInvalidCredentialsException : UnauthorizedException
    {
        public const string DefaultMessage = "Invalid email or password. Please check your credentials and try again.";
       
        public UserInvalidCredentialsException() 
            : base(ErrorKeys.User, DefaultMessage)
        { }
    }
}
