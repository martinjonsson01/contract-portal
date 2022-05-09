﻿using System;
using System.Linq;
using System.Threading;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Infrastructure.Databases;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Users;

[Collection("DatabaseTests")]
public class UserRepositoryTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private EFUserRepository _cut;

    public UserRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        EFDatabaseContext context = _fixture.CreateContext();
        _cut = new EFUserRepository(context, Mock.Of<ILogger<EFUserRepository>>());
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
        // const string adminName = "admin";
        var user = new User();
        var contract1 = new Contract();
        var contract2 = new Contract();
        var contract3 = new Contract();
        _cut.Add(user);

        // Act
        _cut.Add(user.Name, contract1);
        _cut.Add(user.Name, contract2);
        _cut.Add(user.Name, contract3);
        IEnumerable<Contract> contracts = _cut.Fetch(user.Name) !.RecentlyViewContracts;

        // Assert
        contracts.First().Should().BeEquivalentTo(contract3);
        contracts.Last().Should().BeEquivalentTo(contract1);
    }
}
