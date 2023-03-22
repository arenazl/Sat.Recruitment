using System;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.Api.ViewModels
{
    public class UserViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

