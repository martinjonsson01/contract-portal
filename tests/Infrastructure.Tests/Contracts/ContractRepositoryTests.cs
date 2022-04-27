using System;
using System.Linq;

using Application.Contracts;

using Domain.Contracts;

using FluentAssertions.Execution;

using Infrastructure.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Contracts;

public class ContractRepositoryTests
{
    private readonly IContractRepository _cut;

    public ContractRepositoryTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<EFContractRepository>();
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
        _cut = new EFContractRepository(optionsBuilder.Options, Mock.Of<ILogger<EFContractRepository>>());
    }

    [Fact]
    public void RemoveContract_ReturnsFalse_WhenContractDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void RemoveContract_ReturnsTrue_WhenContractExists()
    {
        // Arrange
        Guid id = _cut.All.First().Id;

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void FetchContract_ReturnsContractWithId_WhenGivenExistingId()
    {
        // Arrange
        var contract = new Contract();
        _cut.Add(contract);
        Guid expected = contract.Id;

        // Act
        Guid? actual = _cut.FetchContract(contract.Id)?.Id;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void UpdatingContract_ReplacesContractInRepository_WhenContractAlreadyExists()
    {
        // Arrange
        var oldContract = new Contract { Name = "Old", };
        var newContract = new Contract { Id = oldContract.Id, Name = "New", };
        _cut.Add(oldContract);

        // Act
        _cut.UpdateContract(newContract);
        Contract? fetchedContract = _cut.FetchContract(oldContract.Id);

        // Assert
        using (new AssertionScope())
        {
            fetchedContract.Should().NotBeNull();
            fetchedContract?.Id.Should().Be(oldContract.Id);
            fetchedContract?.Id.Should().Be(newContract.Id);
            fetchedContract?.Name.Should().BeEquivalentTo(newContract.Name);
        }
    }

    [Fact]
    public void UpdatingContract_AddsContractToRepository_WhenContractDoesNotExist()
    {
        // Arrange
        var newContract = new Contract { Name = "New", };

        // Act
        _cut.UpdateContract(newContract);
        Contract? fetchedContract = _cut.FetchContract(newContract.Id);

        // Assert
        using (new AssertionScope())
        {
            fetchedContract.Should().NotBeNull();
            fetchedContract?.Id.Should().Be(newContract.Id);
            fetchedContract?.Name.Should().BeEquivalentTo(newContract.Name);
        }
    }
}
