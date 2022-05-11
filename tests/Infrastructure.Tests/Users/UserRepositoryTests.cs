using System;
using System.Linq;

using Application.Configuration;
using Application.Users;

using Domain.Users;

using Infrastructure.Databases;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Users;

[Collection("DatabaseTests")]
public class UserRepositoryTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private IUserRepository _cut;

    public UserRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        EFDatabaseContext context = _fixture.CreateContext();
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.AdminPassword]).Returns("test_password");
        _cut = new EFUserRepository(context, Mock.Of<ILogger<EFUserRepository>>(), mockEnvironment.Object);
        _cut.EnsureAdminCreated();
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
    public void AfterCreation_AdminUserIsCreated_IfNoAdminUserExistedPreviously()
    {
        // Arrange
        const string adminName = "admin";
        User? admin = _cut.All.FirstOrDefault(user => user.Name == adminName);
        if (admin is not null)
            _cut.Remove(admin.Id);

        // Re-create database.
        EFDatabaseContext context = _fixture.CreateContext();
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.AdminPassword]).Returns("test_password");
        _cut = new EFUserRepository(context, Mock.Of<ILogger<EFUserRepository>>(), mockEnvironment.Object);
        _cut.EnsureAdminCreated();

        // Act
        bool exists = _cut.Exists(adminName);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public void AfterCreation_ThereIsOnlyOneAdmin_IfAdminExistedPreviously()
    {
        // Arrange
        const string adminName = "admin";
        User? admin = _cut.All.FirstOrDefault(user => user.Name == adminName);

        if (admin is null)
            _cut.EnsureAdminCreated();

        // Re-create database.
        EFDatabaseContext context = _fixture.CreateContext();
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.AdminPassword]).Returns("test_password");
        _cut = new EFUserRepository(context, Mock.Of<ILogger<EFUserRepository>>(), mockEnvironment.Object);

        // Act
        IEnumerable<User> admins = _cut.All.Where(user => user.Name == adminName);

        // Assert
        admins.Should().ContainSingle();
    }
}
