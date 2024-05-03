using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.DTOs.Auth
{
    public record SignInRequestDTO(string Email, string Password);
}
