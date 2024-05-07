using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Exceptions
{
    public interface IExceptionBase
    {
        string TypeError { get; init; }
    }
}
