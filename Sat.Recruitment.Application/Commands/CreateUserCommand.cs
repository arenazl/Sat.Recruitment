using System;
using Sat.Recruitment.Domain.Entities;
using MediatR;
using Sat.Recruitment.Domain.ValidationResult;

namespace Sat.Recruitment.Application.Commands
{
	public class CreateUserCommand
	{
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }

        public User ToEntity()
        {
            return new User
            {
                Id = id,    
                Name = this.Name,
                Email = this.Email,
                Address = this.Address,
                Phone = this.Phone,
                UserType = this.UserType,
                Money = this.Money
            };
        }
    }
}

