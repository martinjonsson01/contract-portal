using Application.Users;

using Domain.Users;

using FluentAssertions.Execution;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Tests.Server.Authentication;

public class AuthenticationTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly UsersController _cut;

    public AuthenticationTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        var service = new UserService(_mockRepo.Object);
        _cut = new UsersController(Mock.Of<ILogger<UsersController>>(), service);
    }

    [Fact]
    public void ValidateUser_ReturnsAToken_WhenCorrectUsernameSpecified()
    {
        // Arrange
        var user = new User { Name = "user name", };
        _mockRepo.Setup(repo => repo.Fetch(user.Name)).Returns(user);

        // Act
        IActionResult result = _cut.Validate(user.Name);

        // Assert
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeAssignableTo<AuthenticateResponse>();
        var authResponse = result.As<OkObjectResult>().Value as AuthenticateResponse;
        using (new AssertionScope())
        {
            authResponse.Should().NotBeNull();
            authResponse!.Id.Should().Be(user.Id);
            authResponse!.Username.Should().Be(user.Name);
        }
    }
}
