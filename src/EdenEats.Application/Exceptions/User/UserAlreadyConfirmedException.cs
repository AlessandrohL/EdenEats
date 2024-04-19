using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.User
{
    public sealed class UserAlreadyConfirmedException : ConflictException
    {
        private const string DefaultMessage = "This user has already been confirmed.";

        public UserAlreadyConfirmedException()
            : base(ErrorKeys.User, new string[1] { DefaultMessage })
        { }
    }
}
