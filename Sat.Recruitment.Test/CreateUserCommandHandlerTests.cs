using AutoMapper;
using Moq;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Application.Handlers;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.ValidationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateUserCommandHandler _createUserCommandHandler;

        public CreateUserCommandHandlerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _createUserCommandHandler = new CreateUserCommandHandler(_userServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidUser_ReturnsValidResult()
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

            _userServiceMock.Setup(service => service.CreateUserAsync(command))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _createUserCommandHandler.Handle(command);

            // Assert
            Assert.True(result.IsValid);
            Assert.Same(user, result.Result);
        }

        [Fact]
        public async Task Handle_InvalidUser_ReturnsInvalidResult()
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

            _userServiceMock.Setup(service => service.CreateUserAsync(command))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _createUserCommandHandler.Handle(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("One or more properties cannot be null or empty.", result.ErrorMessage);
        }
    }
}
