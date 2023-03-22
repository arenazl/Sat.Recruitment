using System;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Domain.Entities;
using MediatR;
using AutoMapper;
using System.Net;
using System.Numerics;
using System.Xml.Linq;
using Sat.Recruitment.Domain.ValidationResult;

namespace Sat.Recruitment.Application.Handlers
{

    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<ValidationResult<User>> Handle(CreateUserCommand request)
        {         
            // save user
            var validationResult = await _userService.CreateUserAsync(request);

            return validationResult;
        }
      
    }

}