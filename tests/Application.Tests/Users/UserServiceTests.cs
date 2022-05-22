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
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.JwtSecret]).Returns("test-json-web-token-secret");
        _cut = new UserService(_mockRepo.Object, mockEnvironment.Object);
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
    public void AddingUser_EncryptsPassword_IfPasswordHasValue()
    {
        // Arrange
        const string rawPassword = "abc123";
        var test = new User { Password = rawPassword, };

        // Act
        _cut.Add(test);

        // Assert
        _mockRepo.Verify(repo => repo.Add(It.Is<User>(usr => usr.Password != rawPassword)));
    }

    [Fact]
    public void AddingUser_Throws_WhenUserWithNameAlreadyExists()
    {
        // Arrange
        const string name = "A user's name";
        _mockRepo.Setup(repository => repository.All).Returns(new[] { new User { Name = name, }, });

        // Act
        Action add = () => _cut.Add(new User { Name = name, });

        // Assert
        add.Should().Throw<UserNameTakenException>();
    }

    [Fact]
    public void UpdatingPassword_ObfuscatesPassword_WhenUserAlreadyExists()
    {
        // Arrange
        const string plainTextPassword = "plaintext-password";
        var user = new User { Password = plainTextPassword, };
        _mockRepo.Setup(repository => repository.All).Returns(new[] { user, });
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);

        // Act
        _cut.UpdateUser(user);

        // Assert
        _mockRepo.Verify(repo =>
            repo.UpdateUser(It.Is<User>(updatedUser => updatedUser.Password != plainTextPassword)));
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
    public void ValidatePassword_ShouldBeTrue_WhenPasswordIsUserPassword()
    {
        // Arrange
        const string password = "UserPassword";
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User() { Password = passwordHash, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);
        _mockRepo.Setup(repository => repository.FromName(user.Name)).Returns(user);

        // Act
        AuthenticateResponse authResponse = _cut.Authenticate(user.Name, password);

        // Assert
        authResponse.Should().NotBeNull();
    }

    [Fact]
    public void ValidatePassword_ShouldBeFalse_WhenPasswordIsNotUserPassword()
    {
        // Arrange
        const string correctPassword = "CorrectPassword";
        const string incorrectPassword = "IncorrectPassword";
        string passwordHash1 = BCrypt.Net.BCrypt.HashPassword(correctPassword);
        var user = new User { Name = "User1", Password = passwordHash1, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);
        _mockRepo.Setup(repository => repository.FromName(user.Name)).Returns(user);

        // Act
        Action tryAuthenticate = () => _cut.Authenticate(user.Name, incorrectPassword);

        // Assert
        tryAuthenticate.Should().Throw<InvalidPasswordException>();
    }

    [Fact]
    public void DecryptedPassword_ReturnsTrue_IfCorrectPasswordIsEntered()
    {
        // Arrange
        const string rawPassword = "abc123";
        var test = new User { Password = rawPassword, };
        var encryptedUser = new User();
        _mockRepo.Setup(repo => repo.Add(It.IsAny<User>())).Callback<User>(usr => encryptedUser = usr);
        _cut.Add(test);

        // Act
        bool valid = BCrypt.Net.BCrypt.Verify("abc123", encryptedUser.Password);

        // Assert
        valid.Should().BeTrue();
    }

    [Fact]
    public void DecryptedPassword_ReturnsFalse_IfIncorrectPasswordIsEntered()
    {
        // Arrange
        const string rawPassword = "abc123";
        var test = new User { Password = rawPassword, };
        var encryptedUser = new User();
        _mockRepo.Setup(repo => repo.Add(It.IsAny<User>())).Callback<User>(usr => encryptedUser = usr);
        _cut.Add(test);

        // Act
        bool valid = BCrypt.Net.BCrypt.Verify("wrongPassword", encryptedUser.Password);

        // Assert
        valid.Should().BeFalse();
    }

    [Fact]
    public void Authenticate_ThrowsException_WhenUserDoesNotExist()
    {
        // Act
        Action tryAuthenticate = () => _cut.Authenticate("non-existent user", "randomPassword");

        // Assert
        tryAuthenticate.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Authenticate_ReturnsAuthResponseWithAdminClaim_WhenUserIsAdmin()
    {
        // Arrange
        const string username = "admin";
        const string password = "password";
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { Name = username, Password = passwordHash, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);
        _mockRepo.Setup(repository => repository.FromName(user.Name)).Returns(user);

        // Act
        AuthenticateResponse authResponse = _cut.Authenticate(username, password);

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
