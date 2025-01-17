﻿using System;
using System.Text.RegularExpressions;
using AutoMapper;
using NLog.Fluent;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Domain.ValidationResult;
using Sat.Recruitment.Infrastructure.Logging;

namespace Sat.Recruitment.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ValidationResult<User>> CreateUserAsync(CreateUserCommand command)
        {
            try
            {
                var user = _mapper.Map<User>(command);

                LogUtility.Info("Creating User");

                var validationResult = ValidateUser(user);
                
                if (!validationResult.IsValid)
                {
                    LogUtility.Error(validationResult.ErrorMessage);
                    return validationResult;
                }

                NormalizeMail(ref user);
                
                ApplyAmountBasedInUserType(ref user);

                await _userRepository.AddAsync(user);

                return ValidationResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                LogUtility.Error(ex.ToString());
                return ValidationResult<User>.Fail("An error occurred while creating the user");
            }
        }
      
        private ValidationResult<User> ValidateUser(User user)
        {
            if (!ValidateRequiredProps(user))
            {
                return ValidationResult<User>.Fail("One or more properties cannot be null or empty.");
            }

            if (GetDuplicateProperty(user) != "")
            {
                return ValidationResult<User>.Fail(string.Format("the {0} is already registered", GetDuplicateProperty(user)));
            }

            if (!IsEmailValid(user.Email))
            {
                return ValidationResult<User>.Fail("Email address is not in the correct format.");
            }

            if (!IsUserTypeValid(user.UserType.ToString()))
            {
                return ValidationResult<User>.Fail("User type is not valid.");
            }

            return ValidationResult<User>.Success(user);
        }

        private bool ValidateRequiredProps(User user)
        {
            var propertiesToCheck = new List<string> { "Name", "Phone", "Address", "Email" };
            foreach (var propertyName in propertiesToCheck)
            {
                var propertyValue = user.GetType().GetProperty(propertyName)?.GetValue(user, null);
                if (string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private string GetDuplicateProperty(User user)
        {
            var users = _userRepository.GetAllAsync().Result;
            if (users.Any(u => u.Email == user.Email))
            {
                return "Email";
            }

            if (users.Any(u => u.Name == user.Name))
            {
                return "Name";
            }

            if (users.Any(u => u.Phone == user.Phone))
            {
                return "Phone";
            }

            if (users.Any(u => u.Address == user.Address))
            {
                return "Address";
            }

            return "";
        }

        private bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsUserTypeValid(string userType)
        {
            return userType == "Normal" || userType == "SuperUser" || userType == "Premium";
        }

        private static bool IsValidUserType(string userType)
        {
            switch (userType.ToLower())
            {
                case "Normal":
                case "SuperUser":
                case "Premium":
                    return true;
                default:
                    return false;
            }
        }

        public void ApplyAmountBasedInUserType(ref User user)
        {
            var typeFactors = new Dictionary<UserType, decimal>
            {
                { UserType.Normal, 0.8m},
                { UserType.SuperUser, 0.2m },
                { UserType.Premium, 2m }
            };

            var originalMoney = user.Money;

            if (typeFactors.ContainsKey(user.UserType))
            {
                var factor = typeFactors[user.UserType];
                user.Money = user.Money + (user.Money * factor);
            }

            if (user.UserType == UserType.Normal)
            {
                user.Money = originalMoney > 100 ? originalMoney + (originalMoney * 0.12m) : user.Money;
            }
        }

        public void NormalizeMail(ref User user)
        {
            user.Email = user.Email.Trim();
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                string[] emailParts = user.Email.Split('@');
                if (emailParts.Length == 2)
                {
                    string username = emailParts[0].Replace(".", string.Empty);
                    int plusIndex = username.IndexOf("+");
                    if (plusIndex >= 0)
                    {
                        username = username.Remove(plusIndex);
                    }
                    user.Email = $"{username}@{emailParts[1]}";
                }
            }
        }
    }
}

