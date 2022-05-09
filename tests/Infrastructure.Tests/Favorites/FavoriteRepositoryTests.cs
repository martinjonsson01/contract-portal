using System;
using System.Linq;
using Application.Contracts;
using Application.FavoriteContracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;

using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Contracts;

[Collection("DatabaseTests")]
public class FavoriteRepositoryTests : IClassFixture<TestDatabaseFixture>, IDisposable
{
    private readonly IFavoriteContractRepository _cut;
    private EFDatabaseContext _context;

    public FavoriteRepositoryTests(TestDatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
        _cut = new EFFavoriteRepository(_context, Mock.Of<ILogger<EFFavoriteRepository>>());
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
        User user = new() { Name = "user" };
        Contract contract = new();

        _context.Users.Add(user);
        _context.Contracts.Add(contract);
        _context.SaveChanges();

        // Act
        _cut.Add(user.Name, contract.Id);

        // Assert
        User databaseUser = _context.Users.Where(u => u.Id == user.Id).Include(u => u.Contracts).First();
        databaseUser.Contracts.Should().Contain(contract);
    }

    [Fact]
    public void Add_Throws_WhenContractDoesNotExist()
    {
        // Arrange
        User user = new() { Name = "user" };
        Guid fakeId = Guid.NewGuid();

        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        Action fetch = () => _cut.Add(user.Name, fakeId);

        // Assert
        fetch.Should().Throw<ContractDoesNotExistException>();
    }

    [Fact]
    public void Add_Throws_WhenUserDoesNotExist()
    {
        // Arrange
        string fakeUsername = "fakeUsername";
        Contract contract = new();

        _context.Contracts.Add(contract);
        _context.SaveChanges();

        // Act
        Action fetch = () => _cut.Add(fakeUsername, contract.Id);

        // Assert
        fetch.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Remove_ReturnsTrue_WhenContractIsAFavorite()
    {
        // Arrange
        User user = new() { Name = "user" };
        Contract contract = new();

        user.Contracts.Add(contract);
        _context.Contracts.Add(contract);
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        bool actual = _cut.Remove(user.Name, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Remove_ReturnsFalse_WhenContractIsNotAFavorite()
    {
        // Arrange
        User user = new() { Name = "user" };
        Contract contract = new();

        _context.Contracts.Add(contract);
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        bool actual = _cut.Remove(user.Name, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }
}
