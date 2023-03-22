using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.ViewModels;
using Sat.Recruitment.Application.Commands;
using Sat.Recruitment.Application.Interfaces;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Domain.ValidationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyApp.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICreateUserCommandHandler> _createUserCommandHandlerMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _createUserCommandHandlerMock = new Mock<ICreateUserCommandHandler>();
            _userController = new UserController(_mapperMock.Object, _createUserCommandHandlerMock.Object);
        }

        [Fact]
        public async Task Post_ValidUser_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var userVM = new UserViewModel
            {
                Name = "John Smith",
                Email = "john.smith@example.com",
                Phone = "555-1234",
                Address = "123 Main St",
                UserType = UserType.Normal
            };

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

            _mapperMock.Setup(mapper => mapper.Map<CreateUserCommand>(userVM))
                .Returns(command);

            _createUserCommandHandlerMock.Setup(handler => handler.Handle(command))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _userController.Post(userVM);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(UserController.Post), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            Assert.Same(user, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Post_InvalidUser_ReturnsBadRequestResult()
        {
            // Arrange
            var userVM = new UserViewModel
            {
                Name = "John Smith",
                Email = "john.smith@example.com",
                Phone = "",
                Address = "123 Main St",
                UserType = UserType.Normal
            };

            var command = new CreateUserCommand
            {
                Name = "John Smith",
                Email = "john.smith@example.com",
                Phone = "",
                Address = "123 Main St",
                UserType = UserType.Normal
            };

            var validationResult = ValidationResult<User>.Fail("One or more properties cannot be null or empty.");

            _mapperMock.Setup(mapper => mapper.Map<CreateUserCommand>(userVM))
                .Returns(command);

            _createUserCommandHandlerMock.Setup(handler => handler.Handle(command))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _userController.Post(userVM);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("One or more properties cannot be null or empty.", badRequestResult.Value);
        }
    }
}
