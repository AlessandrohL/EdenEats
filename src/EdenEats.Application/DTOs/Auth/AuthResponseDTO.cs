using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.DTOs
{
    public sealed class AuthResponseDTO
    {
        public string? RefreshToken { get; init; }
        public string? CsrfToken { get; init; }


        public AuthResponseDTO(string refreshToken, string csrfToken)
        {
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(csrfToken))
            {
                throw new ArgumentException($"The {nameof(refreshToken)} or {nameof(csrfToken)} cannot bet null or empty.");
            }

            RefreshToken = refreshToken;
            CsrfToken = csrfToken;
        }

    }
}
