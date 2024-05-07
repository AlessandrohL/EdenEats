using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.Email
{
    public class EmailConfirmationFailedException : ConflictException
    {
        public const string DefaultMessage = "The user's e-mail address could not be confirmed.";

        public EmailConfirmationFailedException(string error)
            : base(ErrorKeys.Email, error)
        { }

        public EmailConfirmationFailedException()
            : this(DefaultMessage)
        { }
    }
}
