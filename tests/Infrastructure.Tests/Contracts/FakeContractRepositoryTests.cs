using System;
using System.Linq;

using Domain.Contracts;

using FluentAssertions.Execution;

using Infrastructure.Contracts;

namespace Infrastructure.Tests.Contracts;
public class FakeContractRepositoryTests
{
    private readonly FakeContractRepository _cut;

    public FakeContractRepositoryTests()
    {
        _cut = new FakeContractRepository();
    }

    [Fact]
    public void AddRecent_AddsContractToQueue_WhenContractNotAlreadyInQueue()
    {
        // Arrange
        var contract = new Contract();

        // Act
        _cut.AddRecent(contract);

        // Assert
        _cut.Recent.Count().Should().Be(1);
    }

    [Fact]
    public void AddRecent_DoesNotAddContractToQueue_WhenContractAlreadyInQueue()
    {
        // Arrange
        var contract = new Contract();

        // Act
        _cut.AddRecent(contract);
        _cut.AddRecent(contract);

        // Assert
        _cut.Recent.Count().Should().Be(1);
    }

    [Fact]
    public void AddRecent_ShouldDequeueTheOldestContract_WhenMoreQueueCapacityIsAdded()
    {
        // Arrange
        var contract1 = new Contract();
        var contract2 = new Contract();
        var contract3 = new Contract();
        var contract4 = new Contract();

        // Act
        _cut.AddRecent(contract1);
        _cut.AddRecent(contract2);
        _cut.AddRecent(contract3);
        _cut.AddRecent(contract4);

        // Assert
        using (new AssertionScope())
        {
            _cut.Recent.Count().Should().Be(3);
            bool containsContract1 = _cut.Recent.Contains(contract1);
            bool containsContract2 = _cut.Recent.Contains(contract2);
            bool containsContract3 = _cut.Recent.Contains(contract3);
            bool containsContract4 = _cut.Recent.Contains(contract4);
            containsContract1.Should().BeFalse();
            containsContract2.Should().BeTrue();
            containsContract3.Should().BeTrue();
            containsContract4.Should().BeTrue();
        }
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
    public void RemoveContract_ReturnTrue_WhenContractExists()
    {
        // Arrange
        Guid id = _cut.All.First().Id;

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void RemoveContract_ShouldRemoveFromRecentlyViewed_WhenContractHasBeenViewed()
    {
        // Arrange
        Contract contract = _cut.All.First();
        Guid id = contract.Id;

        // Act
        _cut.AddRecent(contract);
        _ = _cut.Remove(id);

        // Assert
        _cut.Recent.Count().Should().Be(0);
    }
}
