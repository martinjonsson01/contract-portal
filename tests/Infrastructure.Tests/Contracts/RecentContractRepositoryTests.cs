﻿using System;
using System.Linq;
using Application.Contracts;
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
    public void AddRecent_AddsTheContractCorrectly()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUserToContext(user1);

        var contract1 = new Contract() { Name = "1" };
        _context.Contracts.Add(contract1);

        // Act
        _cut.AddRecent(user1.Name, contract1);
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(user1.Name);

        // Assert
        contracts.First().ContractId.Should().Be(contract1.Id);
    }

    [Fact]
    public void AddingContract_UpdatesTheTime_WhenContractAlreadyExists()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUserToContext(user1);

        var contract1 = new Contract();
        AddContractToContext(contract1);

        // Act
        _cut.AddRecent(user1.Name, contract1);
        var timeBeforeReAdd = _context.Users.First(user => user.Name == user1.Name).RecentlyViewContracts
            .First(recentContract => recentContract.ContractId == contract1.Id)
            .LastViewed;
        _cut.AddRecent(user1.Name, contract1);
        var timeAfterReAdd = _context.Users.First(user => user.Name == user1.Name).RecentlyViewContracts
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
        AddUserToContext(user1);

        var contract1 = new Contract();
        var contract2 = new Contract();
        AddContractToContext(contract1, contract2);
        _cut.AddRecent(user1.Name, contract1);
        _cut.AddRecent(user1.Name, contract2);

        RecentlyViewedContract toRemove = _context.Users.First(user => user.Name == user1.Name).RecentlyViewContracts
            .First(recentContract => recentContract.ContractId == contract1.Id);

        // Act
        _cut.RemoveRecent(toRemove);
        IList<RecentlyViewedContract> contracts = _cut.FetchRecentContracts(user1.Name);

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
        AddUserToContext(user1, user2);

        var contract1 = new Contract();
        AddContractToContext(contract1);
        _cut.AddRecent(user1.Name, contract1);
        _cut.AddRecent(user2.Name, contract1);
        _ = _context.RecentlyViewedContracts.Find(contract1.Id, user1.Id);

        // Act
        _context.Contracts.Remove(contract1);
        _context.SaveChanges();

        // Assert
        _context.Users.First(user => user.Name == user1.Name).RecentlyViewContracts.Should().BeEmpty();
        _context.Users.First(user => user.Name == user2.Name).RecentlyViewContracts.Should().BeEmpty();
    }

    [Fact]
    public void RemoveContract_DoesNotChangeRecentlyViewedForUser_WhenRecentlyViewedDoesNotContainContract()
    {
        // Arrange
        var user1 = new User() { Name = "User1" };
        AddUserToContext(user1);
        var contract1 = new Contract();
        var contract2 = new Contract();
        AddContractToContext(contract1);
        AddContractToContext(contract2);
        _cut.AddRecent(user1.Name, contract1);

        // Act
        _context.Contracts.Remove(contract2);
        _context.SaveChanges();

        _context.Users.First(user => user.Name == user1.Name).RecentlyViewContracts.Should().ContainSingle();
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
