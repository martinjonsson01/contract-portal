using Application.Configuration;
using Application.Users;

using Domain.Users;

using FluentAssertions.Execution;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Server.Tests.Authentication;

public class AuthenticationTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly UsersController _cut;

    public AuthenticationTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.JwtSecret]).Returns("test-json-web-token-secret");
        var service = new UserService(_mockRepo.Object, mockEnvironment.Object);
        _cut = new UsersController(Mock.Of<ILogger<UsersController>>(), service);
    }

    [Fact]
    public void ValidateUser_ReturnsACorrectToken_WhenCorrectUsernameSpecified()
    {
        // Arrange
        var user = new User { Name = "user name", };
        _mockRepo.Setup(repo => repo.Fetch(user.Name)).Returns(user);

        // Act
        IActionResult result = _cut.Authenticate(user.Name);

        // Assert
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeAssignableTo<AuthenticateResponse>();
        var authResponse = result.As<OkObjectResult>().Value as AuthenticateResponse;
        using (new AssertionScope())
        {
            authResponse.Should().NotBeNull();
            authResponse!.Id.Should().Be(user.Id);
            authResponse.Username.Should().Be(user.Name);
            authResponse.Token.Should().NotBeNullOrEmpty();
        }
    }
}
