using System;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Api.ViewModels;
using Sat.Recruitment.Application.Commands;
using AutoMapper;

namespace Sat.Recruitment.Api.Mappings
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserViewModel, CreateUserCommand>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
        }
    }
}



