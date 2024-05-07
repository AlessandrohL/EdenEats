using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.User
{
    public sealed class UserNotFoundException : BadRequestException
    {
        private const string DefaultMessage = "User not found.";

        public UserNotFoundException() 
            : base(ErrorKeys.User, DefaultMessage)
        { }
    }
}
