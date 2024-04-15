using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.User
{
    public sealed class UserAlreadyExistsException : UnprocessableException
    {
        public const string DefaultMessage = "The email address is already in use. Please choose a different email address.";

        public UserAlreadyExistsException(IEnumerable<string> errors)
            : base(ErrorKeys.User, errors)
        { }

        public UserAlreadyExistsException(string error)
            : this(new string[1] { error })
        { }

        public UserAlreadyExistsException()
            : this(new string[1] { DefaultMessage })
        { }
    }
}
