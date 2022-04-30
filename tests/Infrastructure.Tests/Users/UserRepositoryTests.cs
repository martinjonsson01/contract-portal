using System;
using System.Linq;

using Application.Users;

using Domain.Users;

namespace Infrastructure.Tests.Users;

public class UserRepositoryTests : IClassFixture<TestUserDatabaseFixture>
{
    private readonly TestUserDatabaseFixture _fixture;
    private IUserRepository _cut;

    public UserRepositoryTests(TestUserDatabaseFixture fixture)
    {
        _fixture = fixture;
        _cut = _fixture.CreateContext();
    }

    [Fact]
    public void RemoveUser_ReturnsFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void RemoveUser_ReturnsTrue_WhenUserDoesExist()
    {
        // Arrange
        User user = new();
        _cut.Add(user);

        // Act
        bool actual = _cut.Remove(user.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void AfterCreation_DefaultUserIsCreated_IfNoDefaultUserExistedPreviously()
    {
        // Arrange
        const string defaultUsername = "default user";
        User? defaultUser = _cut.All.FirstOrDefault(user => user.Name == defaultUsername);
        if (defaultUser is not null)
            _cut.Remove(defaultUser.Id);

        // Re-create database.
        _cut = _fixture.CreateContext();

        // Act
        bool exists = _cut.UserExists(defaultUsername);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public void AfterCreation_ThereIsOnlyOneDefaultUser_IfDefaultUserExistedPreviously()
    {
        // Arrange
        const string defaultUsername = "default user";
        User? defaultUser = _cut.All.FirstOrDefault(user => user.Name == defaultUsername);

        if (defaultUser is null) // Ensure user exists.
            _cut.Add(new User { Name = defaultUsername, });

        _cut = _fixture.CreateContext(); // Re-create database.

        // Act
        IEnumerable<User> defaultUsers = _cut.All.Where(user => user.Name == defaultUsername);

        // Assert
        defaultUsers.Should().ContainSingle();
    }
}
