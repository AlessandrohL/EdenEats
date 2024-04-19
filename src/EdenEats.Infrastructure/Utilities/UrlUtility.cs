using EdenEats.Application.Contracts.Utilities;
using Microsoft.AspNetCore.WebUtilities;
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
        public const string Base64UrlRegex = @"^([0-9a-zA-Z_\-]{4})*([0-9a-zA-Z_\-]{2})?$";

        public string DecodeBase64Url(string base64Url)
        {
            var bytes = WebEncoders.Base64UrlDecode(base64Url);
            var decodedToken = Encoding.UTF8.GetString(bytes);

            return decodedToken;
        }

        public string EncodeUrlToBase64(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var encoded = WebEncoders.Base64UrlEncode(bytes);
            return encoded;
        }

        public static bool ValidateBase64Url(string base64Url)
        {
            return Regex.IsMatch(base64Url, Base64UrlRegex);
        }
    }
}
