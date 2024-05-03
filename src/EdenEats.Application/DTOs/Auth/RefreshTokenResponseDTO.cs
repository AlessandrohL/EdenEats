using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.DTOs.Auth
{
    public sealed class RefreshTokenResponseDTO
    {
        public string? CsrfToken { get; init; }

        public RefreshTokenResponseDTO(string csrfToken)
        {
            if (string.IsNullOrEmpty(csrfToken))
            {
                throw new ArgumentException($"The {nameof(csrfToken)} cannot bet null or empty.");
            }

            CsrfToken = csrfToken;
        }
    }
}
