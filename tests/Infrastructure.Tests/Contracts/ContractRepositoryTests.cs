using System;
using System.Linq;

using Application.Contracts;

using Domain.Contracts;

using Infrastructure.Contracts;

namespace Infrastructure.Tests.Contracts;

public class ContractRepositoryTests
{
    private readonly IContractRepository _cut;

    public ContractRepositoryTests()
    {
        _cut = new FakeContractRepository();
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
}
