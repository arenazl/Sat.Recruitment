using System;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> AddAsync(User user);
     
    }
}

