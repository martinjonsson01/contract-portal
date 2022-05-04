using Application.Exceptions;
using Application.Users;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Tests.Server.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _cut;
    private readonly Mock<IUserService> _mockUsers;

    public UsersControllerTests()
    {
        _mockUsers = new Mock<IUserService>();
        _cut = new UsersController(Mock.Of<ILogger<UsersController>>(), _mockUsers.Object);
    }

    [Fact]
    public void Create_ReturnsBadRequest_IfIDAlreadyTaken()
    {
        // Arrange
        var user = new User();
        _mockUsers.Setup(service => service.Add(user)).Throws<IdentifierAlreadyTakenException>();

        // Act
        IActionResult actual = _cut.Create(user);

        // Assert
        actual.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void Create_ReturnsOk_IfIDIsUnique()
    {
        // Arrange

        // Act
        IActionResult actual = _cut.Create(new User());

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Validate_ReturnsOk_IfUserExistsAndPasswordIsCorrect()
    {
        // Arrange
        var user = new User() { Name = "user", Password = "password", };
        _mockUsers.Setup(service => service.UserExists(user.Name)).Returns(true);
        _mockUsers.Setup(service => service.ValidPassword(user.Name, user.Password)).Returns(true);

        // Act
        IActionResult actual = _cut.Validate(user);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Validate_ReturnsBadRequest_IfUserDoesNotExist()
    {
        // Arrange
        var username = new User() { Name = "user", };
        _mockUsers.Setup(service => service.UserExists(username.Name)).Returns(false);

        // Act
        IActionResult actual = _cut.Validate(username);

        // Assert
        actual.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void Remove_ReturnsOk_WhenIDExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUsers.Setup(service => service.Remove(id)).Returns(true);

        // Act
        IActionResult actual = _cut.Remove(id);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Remove_CallsUserService_WhenIDExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUsers.Setup(service => service.Remove(id)).Returns(true);

        // Act
        IActionResult actual = _cut.Remove(id);

        // Assert
        _mockUsers.Verify(o => o.Remove(id), Times.Once);
    }

    [Fact]
    public void Remove_ReturnsNotFound_WhenIDDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUsers.Setup(service => service.Remove(id)).Returns(false);

        // Act
        IActionResult actual = _cut.Remove(id);

        // Assert
        actual.Should().BeOfType<NotFoundResult>();
    }
}
