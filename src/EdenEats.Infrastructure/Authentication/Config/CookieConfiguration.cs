using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Authentication.Config
{
    public sealed class CookieConfiguration
    {
        public string? CookieName { get; set; }
        public double CookieLifetimeDays { get; set; }

    }
}
