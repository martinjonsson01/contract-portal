using System;
using System.Linq;
using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Infrastructure.Databases;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Contracts;

[Collection("DatabaseTests")]
public class RecentContractRepositoryTests : IClassFixture<TestDatabaseFixture>, IDisposable
{
    private readonly EFRecentContractRepository _cut;
    private readonly EFDatabaseContext _context;

    public RecentContractRepositoryTests(TestDatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
        _cut = new EFRecentContractRepository(_context, Mock.Of<ILogger<EFRecentContractRepository>>());
        _context.Database.BeginTransaction();
    }

    [Fact]
    public void FetchRecentContracts_KeepsIdIntact_WhenAddedWithId()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUsersToContext(user1);

        var contract1 = new Contract() { Name = "1" };
        _context.Contracts.Add(contract1);
        _cut.Add(user1.Id, contract1);

        // Act
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(user1.Id);

        // Assert
        contracts.First().ContractId.Should().Be(contract1.Id);
    }

    [Fact]
    public void AddingContract_UpdatesTheTime_WhenContractAlreadyExists()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUsersToContext(user1);

        var contract1 = new Contract();
        AddContractsToContext(contract1);

        // Act
        _cut.Add(user1.Id, contract1);
        var timeBeforeReAdd = _context.Users.First(user => user.Id == user1.Id).RecentlyViewContracts
            .First(recentContract => recentContract.ContractId == contract1.Id)
            .LastViewed;
        _cut.Add(user1.Id, contract1);
        var timeAfterReAdd = _context.Users.First(user => user.Id == user1.Id).RecentlyViewContracts
            .First(recentContract => recentContract.ContractId == contract1.Id)
            .LastViewed;

        // Assert
        timeBeforeReAdd.Should().BeBefore(timeAfterReAdd);
    }

    [Fact]
    public void RemoveRecent_RemovesSpecifiedContract_WhenThereAreMultipleRecents()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUsersToContext(user1);

        var contract1 = new Contract();
        var contract2 = new Contract();
        AddContractsToContext(contract1, contract2);
        _cut.Add(user1.Id, contract1);
        _cut.Add(user1.Id, contract2);

        RecentlyViewedContract toRemove = _context.Users.First(user => user.Id == user1.Id).RecentlyViewContracts
            .First(recentContract => recentContract.ContractId == contract1.Id);

        // Act
        _cut.Remove(toRemove);
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(user1.Id);

        // Assert
        contracts.First().ContractId.Should().Be(contract2.Id);
        contracts.Should().ContainSingle();
    }

    [Fact]
    public void Removing_RemovesRecentContractFromMultipleUsers_WhenMultipleUsersHaveViewedItRecently()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        var user2 = new User() { Name = "User2" };
        AddUsersToContext(user1, user2);

        var contract1 = new Contract();
        AddContractsToContext(contract1);
        _cut.Add(user1.Id, contract1);
        _cut.Add(user2.Id, contract1);

        // Act
        _context.Contracts.Remove(contract1);
        _context.SaveChanges();

        // Assert
        _context.Users.First(user => user.Id == user1.Id).RecentlyViewContracts.Should().BeEmpty();
        _context.Users.First(user => user.Id == user2.Id).RecentlyViewContracts.Should().BeEmpty();
    }

    [Fact]
    public void RemoveContract_DoesNotChangeRecentlyViewedForUser_WhenRecentlyViewedDoesNotContainContract()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUsersToContext(user1);
        var contract1 = new Contract();
        var contract2 = new Contract();
        AddContractsToContext(contract1);
        AddContractsToContext(contract2);
        _cut.Add(user1.Id, contract1);

        // Act
        _context.Contracts.Remove(contract2);
        _context.SaveChanges();

        _context.Users.First(user => user.Id == user1.Id).RecentlyViewContracts.Should().ContainSingle();
    }

    [Fact]
    public void FetchingRecent_ThrowUserDoesNotExist_WhenUserToFetchFromDoesNotExist()
    {
        // Arrange
        var user1 = new User();

        // Act
        Action fetch = () => _cut.FetchRecentContracts(user1.Id);

        // Assert
        fetch.Should().Throw<UserDoesNotExistException>();
    }

    public void Dispose()
    {
        _context.ChangeTracker.Clear();
        _context.Dispose();
    }

    private void AddUsersToContext(params User[] users)
    {
        foreach (User user in users)
        {
            _context.Users.Add(user);
        }

        _context.SaveChanges();
    }

    private void AddContractsToContext(params Contract[] contracts)
    {
        foreach (Contract contract in contracts)
        {
            _context.Contracts.Add(contract);
        }

        _context.SaveChanges();
    }
}
