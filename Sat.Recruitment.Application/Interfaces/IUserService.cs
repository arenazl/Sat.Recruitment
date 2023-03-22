using System;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.ValidationResult;

namespace Sat.Recruitment.Application.Interfaces
{
    public interface IUserService
    {
        Task<ValidationResult<User>> CreateUserAsync(CreateUserCommand command);
    } 
}

