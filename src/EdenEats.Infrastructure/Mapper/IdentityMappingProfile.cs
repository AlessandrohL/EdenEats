using AutoMapper;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Mapper
{
    public sealed class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<ApplicationUser, UserIdentityDTO>()
                .ReverseMap();
        }
    }
}
