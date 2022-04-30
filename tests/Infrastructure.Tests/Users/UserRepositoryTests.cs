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
    public void AfterCreation_AdminIsCreated_IfNoAdminExistedPreviously()
    {
        // Arrange
        const string adminUsername = "admin";
        User? admin = _cut.All.FirstOrDefault(user => user.Name == adminUsername);
        if (admin is not null)
            _cut.Remove(admin.Id);

        // Re-create database.
        _cut = _fixture.CreateContext();

        // Act
        bool exists = _cut.UserExists(adminUsername);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public void AfterCreation_ThereIsOnlyOneAdmin_IfAdminExistedPreviously()
    {
        // Arrange
        const string adminUsername = "admin";
        User? admin = _cut.All.FirstOrDefault(user => user.Name == adminUsername);

        if (admin is null) // Ensure admin exists.
            _cut.Add(new User { Name = adminUsername, });

        _cut = _fixture.CreateContext(); // Re-create database.

        // Act
        IEnumerable<User> admins = _cut.All.Where(user => user.Name == adminUsername);

        // Assert
        admins.Should().ContainSingle();
    }
}
