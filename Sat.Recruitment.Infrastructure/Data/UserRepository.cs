using System;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Infrastructure.Logging;


namespace Sat.Recruitment.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _users = new List<User>();

        private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Users.txt");

        public async Task<User> AddAsync(User user)
        {
            user.Id = _users.Count()+1;
            _users.Add(user);
            await SaveToFileAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            await LoadFromFileAsync();
            return _users;
        }

        private static async Task LoadFromFileAsync()
        {
            _users = new List<User>();

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File not found: {_filePath}");
            }

            var lines = await File.ReadAllLinesAsync(_filePath);
            foreach (var line in lines)
            {
                var values = line.Split(',');

                if (values.Length != 6)
                {
                    continue;
                }
                var user = new User
                {
                    Name = values[0],
                    Email = values[1],
                    Phone = values[2],
                    Address = values[3],
                    UserType = ParseUserType(values[4]),
                    Money = int.TryParse(values[5], out var moneyValue) ? moneyValue : 0
                };

                _users.Add(user);
            }
        }

        private static async Task SaveToFileAsync()
        {
            LogUtility.Info("Persisting User in the text file");
            var lines = _users.Select(u => $"{u.Name},{u.Email},{u.Address},{u.Phone},{u.UserType},{u.Money}");
            await File.WriteAllLinesAsync(_filePath, lines);
        }

        public static UserType ParseUserType(string userTypeText)
        {
            UserType userType;
            if (!Enum.TryParse(userTypeText, true, out userType))
            {
                LogUtility.Error($"Invalid user type: {userTypeText}");
                throw new ArgumentException($"Invalid user type: {userTypeText}");
            }

            return userType;
        }
    }


}


