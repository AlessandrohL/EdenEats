using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Authentication.Config
{
    public static class CsrfConfiguration
    {
        public const string HeaderName = "X-XSRF-TOKEN";
    }
}
