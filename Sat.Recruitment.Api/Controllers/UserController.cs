using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Application.Commands;
using System.Threading.Tasks;
using AutoMapper;
using Sat.Recruitment.Api.ViewModels;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Infrastructure.Logging;
using Sat.Recruitment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Sat.Recruitment.Application.Handlers;
using MediatR;
using System.Threading;
using Sat.Recruitment.Domain.ValidationResult;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICreateUserCommandHandler _createUserCommandHandler;

        public UserController(IMapper mapper, ICreateUserCommandHandler createUserCommandHandler)
        {
            _mapper = mapper;
            _createUserCommandHandler = createUserCommandHandler;
        }

        [HttpPost]
        public async Task<ActionResult<ValidationResult<User>>> Post(UserViewModel userVM)
        {
            var command = _mapper.Map<CreateUserCommand>(userVM);

            LogUtility.Info("calling command handler");

            var validationResult = await _createUserCommandHandler.Handle(command);

            if (!validationResult.IsValid)
            {          
                return BadRequest(validationResult.ErrorMessage);
            }

            return CreatedAtAction(nameof(Post), new { id = validationResult.Result.Id }, validationResult.Result);
        }
    }
    
}

