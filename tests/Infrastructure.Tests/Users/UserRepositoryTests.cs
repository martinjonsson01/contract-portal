using System;
using System.Linq;
using System.Threading;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Infrastructure.Databases;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Users;

[Collection("DatabaseTests")]
public class UserRepositoryTests : IClassFixture<TestDatabaseFixture>, IDisposable
{
    private readonly TestDatabaseFixture _fixture;
    private EFUserRepository _cut;
    private EFDatabaseContext _context;

    public UserRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();
        _cut = new EFUserRepository(_context, Mock.Of<ILogger<EFUserRepository>>());
        _cut.EnsureAdminCreated();
        _context.Database.BeginTransaction();
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
        _cut = new EFUserRepository(context, Mock.Of<ILogger<EFUserRepository>>());
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

        if (admin is null) // Ensure admin exists.
            _cut.Add(new User { Name = adminName, });

        // Re-create database.
        EFDatabaseContext context = _fixture.CreateContext();
        _cut = new EFUserRepository(context, Mock.Of<ILogger<EFUserRepository>>());

        // Act
        IEnumerable<User> admins = _cut.All.Where(user => user.Name == adminName);

        // Assert
        admins.Should().ContainSingle();
    }

    [Fact]
    public void AddRecent_ContractsAreInExpectedOrderAfterBeingAdded()
    {
        // Arrange
        const string adminName = "admin";
        var contract1 = new Contract() { Name = "1" };
        _context.Contracts.Add(contract1);

        // Act
        _cut.AddRecent(adminName, contract1);
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(adminName);

        // Assert
        contracts.First().ContractId.Should().Be(contract1.Id);
    }

    [Fact]
    public void AddRecent_ReAddingAnExistingContractPlacesItFirst()
    {
        // Arrange
        const string adminName = "admin";
        var contract1 = new Contract();
        var contract2 = new Contract();
        _context.Contracts.Add(contract1);
        _context.Contracts.Add(contract2);

        // Act
        _cut.AddRecent(adminName, contract1);
        _cut.AddRecent(adminName, contract2);
        _cut.AddRecent(adminName, contract1);
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(adminName);

        // Assert
        contracts.First().ContractId.Should().Be(contract1.Id);
        contracts.Last().ContractId.Should().Be(contract2.Id);
    }

    [Fact]
    public void RemoveLast_RemovesTheLastAddedContract()
    {
        // Arrange
        const string adminName = "admin";
        var contract1 = new Contract();
        var contract2 = new Contract();
        _context.Contracts.Add(contract1);
        _context.Contracts.Add(contract2);
        _cut.AddRecent(adminName, contract1);
        _cut.AddRecent(adminName, contract2);

        RecentlyViewedContract toRemove = _cut.Fetch(adminName) !.RecentlyViewContracts
            .First(recentContract => recentContract.ContractId == contract1.Id);

        // Act
        _cut.RemoveRecent(toRemove);
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(adminName);

        // Assert
        contracts.First().ContractId.Should().Be(contract2.Id);
        contracts.Should().ContainSingle();
    }

    [Fact]
    public void RemoveContract_RemovesAllContractFromAllUser()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        var user2 = new User() { Name = "User2" };
        _cut.Add(user1);
        _cut.Add(user2);
        var contract1 = new Contract();
        _context.Contracts.Add(contract1);
        _context.SaveChanges();
        _cut.AddRecent(user1.Name, contract1);
        _cut.AddRecent(user2.Name, contract1);
        _ = _context.RecentlyViewedContracts.Find(contract1.Id, user1.Id);

        // Act
        _context.Contracts.Remove(contract1);
        _context.SaveChanges();

        // Assert
        _cut.Fetch(user1.Name) !.RecentlyViewContracts.Should().BeEmpty();
        _cut.Fetch(user2.Name) !.RecentlyViewContracts.Should().BeEmpty();
    }

    [Fact]
    public void RemoveContract_DoesNotChangeRecentlyViewedForUser_WhenRecentlyViewedDoesNotContainContract()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        _cut.Add(user1);
        var contract1 = new Contract();
        var contract2 = new Contract();
        _context.Contracts.Add(contract1);
        _context.Contracts.Add(contract2);
        _context.SaveChanges();
        _cut.AddRecent(user1.Name, contract1);

        // Act
        _context.Contracts.Remove(contract2);
        _context.SaveChanges();

        _cut.Fetch(user1.Name) !.RecentlyViewContracts.Should().ContainSingle();
    }

    public void Dispose()
    {
        _context.ChangeTracker.Clear();
        _context.Dispose();
    }
}
