using System;
using System.Linq;

using Application.Configuration;
using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;

using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Users;

[Collection("DatabaseTests")]
public class UserRepositoryFavoriteTests : IClassFixture<TestDatabaseFixture>, IDisposable
{
    private readonly EFDatabaseContext _context;
    private IUserRepository _cut;

    public UserRepositoryFavoriteTests(TestDatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.AdminPassword]).Returns("test_password");
        _cut = new EFUserRepository(_context, Mock.Of<ILogger<EFUserRepository>>(), mockEnvironment.Object);
        _cut.EnsureAdminCreated();
    }

    public void Dispose()
    {
        _context.Contracts.RemoveRange(_context.Contracts);
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();
        _context.Dispose();
    }

    [Fact]
    public void Add_MarksContractAsFavorite_WhenUserAndContractExist()
    {
        // Arrange
        User user = new() { Name = "user", };
        Contract contract = new();

        _context.Users.Add(user);
        _context.Contracts.Add(contract);
        _context.SaveChanges();

        // Act
        _cut.AddFavorite(user.Id, contract.Id);

        // Assert
        User databaseUser = _context.Users.Where(u => u.Id == user.Id).Include(u => u.Favorites).First();
        databaseUser.Favorites.Should().Contain(contract);
    }

    [Fact]
    public void Add_Throws_WhenContractDoesNotExist()
    {
        // Arrange
        User user = new() { Name = "user", };
        Guid fakeId = Guid.NewGuid();

        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        Action fetch = () => _cut.AddFavorite(user.Id, fakeId);

        // Assert
        fetch.Should().Throw<ContractDoesNotExistException>();
    }

    [Fact]
    public void Add_Throws_WhenUserDoesNotExist()
    {
        // Arrange
        var nonexistentUserId = Guid.NewGuid();
        Contract contract = new();

        _context.Contracts.Add(contract);
        _context.SaveChanges();

        // Act
        Action fetch = () => _cut.AddFavorite(nonexistentUserId, contract.Id);

        // Assert
        fetch.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Remove_ReturnsTrue_WhenContractIsAFavorite()
    {
        // Arrange
        User user = new() { Name = "user", };
        Contract contract = new();

        user.Favorites.Add(contract);
        _context.Contracts.Add(contract);
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        bool actual = _cut.RemoveFavorite(user.Id, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Remove_ReturnsFalse_WhenContractIsNotAFavorite()
    {
        // Arrange
        User user = new() { Name = "user", };
        Contract contract = new();

        _context.Contracts.Add(contract);
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        bool actual = _cut.RemoveFavorite(user.Id, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }
}
