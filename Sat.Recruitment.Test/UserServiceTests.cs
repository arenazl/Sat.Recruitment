using AutoMapper;
using Moq;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Application.Services;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Domain.ValidationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyApp.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_ReturnsValidResult()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "John Smith",
                Email = "john.smith@example.com",
                Phone = "555-1234",
                Address = "123 Main St",
                UserType = UserType.Normal
            };

            var user = new User
            {
                Id = 1,
                Name = "John Smith",
                Email = "john.smith@example.com",
                Phone = "555-1234",
                Address = "123 Main St",
                UserType = UserType.Normal
            };

            var validationResult = ValidationResult<User>.Success(user);

            _mapperMock.Setup(mapper => mapper.Map<User>(command))
                .Returns(user);

            _userRepositoryMock.Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<User>());

            _userRepositoryMock.Setup(repository => repository.AddAsync(user))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.CreateUserAsync(command);

            // Assert
            Assert.True(result.IsValid);
            Assert.Same(user, result.Result);
        }

        [Fact]
        public async Task CreateUserAsync_InvalidUser_ReturnsInvalidResult()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "",
                Email = "john.smith@example.com",
                Phone = "555-1234",
                Address = "123 Main St",
                UserType = UserType.Normal
            };

            var validationResult = ValidationResult<User>.Fail("One or more properties cannot be null or empty.");

            _mapperMock.Setup(mapper => mapper.Map<User>(command))
                .Returns(new User());

            // Act
            var result = await _userService.CreateUserAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("One or more properties cannot be null or empty.", result.ErrorMessage);
        }
    }
}
