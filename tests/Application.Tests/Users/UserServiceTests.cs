using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

using Application.Configuration;
using Application.Exceptions;
using Application.Users;

using Domain.Users;

using FluentAssertions.Execution;

using Microsoft.Extensions.Configuration;

namespace Application.Tests.Users;

public class UserServiceTests
{
    private readonly UserService _cut;
    private readonly Mock<IUserRepository> _mockRepo;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        var mockEnvironment = new Mock<IEnvironmentConfiguration>();
        mockEnvironment.Setup(env => env.JwtSecret).Returns("test-json-web-token-secret");
        _cut = new UserService(_mockRepo.Object, Mock.Of<IConfiguration>(), mockEnvironment.Object);
    }

    [Fact]
    public void AddingUser_ThrowsIDException_IfIDAlreadyTaken()
    {
        // Arrange
        var user = new User();
        _mockRepo.Setup(repository => repository.All).Returns(new[] { user, });

        // Act
        Action add = () => _cut.Add(user);

        // Assert
        add.Should().Throw<IdentifierAlreadyTakenException>();
    }

    [Fact]
    public void AddingUser_DoesNotThrow_IfIDIsUnique()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<User>());

        // Act
        Action add = () => _cut.Add(new User());

        // Assert
        add.Should().NotThrow();
    }

    [Fact]
    public void RemovingUser_DoesReturnTrue_WhenAUserExists()
    {
        // Arrange
        var user = new User();
        _mockRepo.Setup(repository => repository.Remove(user.Id)).Returns(true);

        // Act
        bool actual = _cut.Remove(user.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void RemovingUser_DoesReturnFalse_WhenNoUsersExists()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<User>());

        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void FetchAllUsers_ReturnsAllUsersInTheDatabase()
    {
        // Arrange
        const int numberOfUsers = 10;
        List<User> mockUsers = new Faker<User>().Generate(numberOfUsers);
        _mockRepo.Setup(repository => repository.All).Returns(mockUsers);

        // Act
        IEnumerable<User> users = _cut.FetchAllUsers();

        // Assert
        users.Should().HaveCount(numberOfUsers);
    }

    [Fact]
    public void ValidatePassword_shouldBeTrue_WhenPasswordIsUserPassword()
    {
        // Arrange
        const string password = "UserPassword";
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User() { Password = passwordHash, };
        _mockRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

        // Act
        bool valid = _cut.ValidPassword(user.Name, password);

        // Assert
        valid.Should().BeTrue();
    }

    [Fact]
    public void ValidatePassword_shouldBeFalse_WhenPasswordIsNotUserPassword()
    {
        // Arrange
        const string password1 = "User1Password";
        const string password2 = "User2Password";
        string passwordHash1 = BCrypt.Net.BCrypt.HashPassword(password1);
        var user = new User() { Name = "User1", Password = passwordHash1, };
        _mockRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

        // Act
        bool valid = _cut.ValidPassword(user.Name, password2);

        // Assert
        valid.Should().BeFalse();
    }

    [Fact]
    public void UserExists_ReturnsTrue_IfUserExistsInRepository()
    {
        // Arrange
        string username = "user";
        _mockRepo.Setup(repository => repository.Exists(username)).Returns(true);

        // Act
        bool actual = _cut.UserExists(username);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void UserExists_ReturnsFalse_IfUserDoesNotExistInRepository()
    {
        // Arrange
        string username = "user";
        _mockRepo.Setup(repository => repository.Exists(username)).Returns(false);

        // Act
        bool actual = _cut.UserExists(username);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Authenticate_ReturnsAuthResponse_WhenUserExists()
    {
        // Arrange
        const string username = "user";
        var user = new User { Name = username, };
        _mockRepo.Setup(repository => repository.Fetch(username)).Returns(user);

        // Act
        AuthenticateResponse authResponse = _cut.Authenticate(username);

        // Assert
        authResponse.Should().NotBeNull();
    }

    [Fact]
    public void Authenticate_ThrowsException_WhenUserDoesNotExist()
    {
        // Arrange

        // Act
        Action tryAuthenticate = () => _cut.Authenticate("non-existent user");

        // Assert
        tryAuthenticate.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Authenticate_ReturnsAuthResponseWithAdminClaim_WhenUserIsAdmin()
    {
        // Arrange
        const string username = "admin";
        var user = new User { Name = username, };
        _mockRepo.Setup(repository => repository.Fetch(username)).Returns(user);

        // Act
        AuthenticateResponse authResponse = _cut.Authenticate(username);

        // Assert
        using (new AssertionScope())
        {
            authResponse.Should().NotBeNull();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(authResponse.Token);
            string? claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            claimValue.Should().Be("true");
        }
    }
}
