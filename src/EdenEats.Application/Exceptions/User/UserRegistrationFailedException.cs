using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.User
{
    public class UserRegistrationFailedException : UnprocessableException
    {
        public UserRegistrationFailedException(IEnumerable<string> errors)
            : base(ErrorKeys.User, string.Join(',', errors))
        { }
    }
}
