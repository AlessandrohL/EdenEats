using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Exceptions.BaseExceptions
{
    public abstract class ConflictException : Exception, IExceptionBase
    {
        public string TypeError { get; init; } = null!;
        public ConflictException(string typeError, string error)
            : base(error)
        {
            TypeError = typeError;
        }
    }
}
