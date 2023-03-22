using System;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.ValidationResult;

namespace Sat.Recruitment.Application.Interfaces
{
    public interface ICreateUserCommandHandler
    {
        Task<ValidationResult<User>> Handle(CreateUserCommand command);
    }
}

