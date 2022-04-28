using System;

using Application.Users;
using Domain.Users;

namespace Infrastructure.Tests.Users;

public class UserRepositoryTests : IClassFixture<TestUserDatabaseFixture>
{
    private readonly IUserRepository _cut;

    public UserRepositoryTests(TestUserDatabaseFixture fixture)
    {
        _cut = fixture.CreateContext();
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
}
