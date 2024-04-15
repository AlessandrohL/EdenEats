using EdenEats.Application.Contracts.Utilities;
using NeoSmart.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Utilities
{
    public sealed class UrlUtility : IUrlUtility
    {
        public byte[] DecodeBase64Url(string base64Url)
        {
            var decoded = UrlBase64.Decode(base64Url);
            return decoded;
        }

        public byte[] EncodeUrlToBase64(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var encoded = UrlBase64.EncodeUtf8(bytes, PaddingPolicy.Discard);
            return encoded;
        }

        public bool ValidateBase64Url(string base64Url)
        {
            var regex = @"^([0-9a-zA-Z_\-]{4})*([0-9a-zA-Z_\-]{2})?$";
            return Regex.IsMatch(base64Url, regex);
        }
    }
}
