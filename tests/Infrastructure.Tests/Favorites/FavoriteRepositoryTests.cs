using System;
using System.Linq;
using Application.Contracts;
using Application.FavoriteContracts;
using Domain.Contracts;
using Domain.Users;

using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Contracts;

[Collection("DatabaseTests")]
public class FavoriteRepositoryTests : IClassFixture<TestDatabaseFixture>
{
    private readonly IFavoriteContractRepository _cut;
    private EFDatabaseContext _context;

    public FavoriteRepositoryTests(TestDatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
        _cut = new EFFavoriteRepository(_context, Mock.Of<ILogger<EFFavoriteRepository>>());
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
        var contracts = _context.Users.Where(u => u.Id == user.Id).Include(c => c.Contracts);
        contracts.Where(c => c.Id == contract.Id).Should().NotBeNull();
    }

    [Fact]
    public void Add_DoesNotMarkContractAsFavorite_WhenContractDoesNotExist()
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
        var id = Guid.NewGuid();
        string userName = "notAUser";

        // Act
        bool actual = _cut.Remove(userName, id);

        // Assert
        actual.Should().BeFalse();
    }
}
