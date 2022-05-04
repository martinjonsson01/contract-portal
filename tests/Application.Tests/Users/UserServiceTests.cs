using System;

using Application.Exceptions;
using Application.Users;

using Domain.Users;

namespace Application.Tests.Users;

public class UserServiceTests
{
    private readonly UserService _cut;
    private readonly Mock<IUserRepository> _mockRepo;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _cut = new UserService(_mockRepo.Object);
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
        var user1 = new User() { Name = "User1", Password = password1, };
        var user2 = new User() { Name = "User2", Password = password2, };
        _mockRepo.Setup(repository => repository.Fetch(user1.Name)).Returns(user1);

        // Act
        bool valid = _cut.ValidPassword(user1.Name, password2);

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
}
