using AutoMapper;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserRegistrationDTO, Customer>();
        }
    }
}
