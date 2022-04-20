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
    public void CreateUser_ReturnsBadRequest_IfIDAlreadyTaken()
    {
        // Arrange
        var user = new User();
        _mockUsers.Setup(service => service.Add(user)).Throws<IdentifierAlreadyTakenException>();

        // Act
        IActionResult actual = _cut.CreateUser(user);

        // Assert
        actual.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void CreateUser_ReturnsOk_IfIDIsUnique()
    {
        // Arrange

        // Act
        IActionResult actual = _cut.CreateUser(new User());

        // Assert
        actual.Should().BeOfType<OkResult>();
    }
}
