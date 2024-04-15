using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Storage
{
    public sealed class CloudinaryConfiguration
    {
        public string? CloudName { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
        public string? Folder { get; set; }

        public Account GetAccount()
            => new (CloudName, ApiKey, ApiSecret);

        public Cloudinary GetCloudinary() => new(GetAccount());
    }
}
