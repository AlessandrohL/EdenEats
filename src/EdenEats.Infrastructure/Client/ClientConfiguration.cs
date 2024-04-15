using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Client
{
    public sealed class ClientConfiguration
    {
        public string? BaseUrl { get; set; }
        public string? ConfirmationEndpoint { get; set; }
    }
}
