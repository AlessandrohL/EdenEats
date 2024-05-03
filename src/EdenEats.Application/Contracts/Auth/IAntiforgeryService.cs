using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Auth
{
    public interface IAntiforgeryService
    {
        string GenerateToken();

    }
}
