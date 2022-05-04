using System;
using System.Linq;

using Application.Contracts;

using Domain.Contracts;

using FluentAssertions.Execution;

namespace Application.Tests.Contracts;

public class RecentContractServiceTests
{
    private readonly IRecentContractService _cut;

    public RecentContractServiceTests()
    {
        _cut = new RecentContractService();
    }

    [Fact]
    public void AddRecent_AddsContractToQueue_WhenContractNotAlreadyInQueue()
    {
        // Arrange
        var contract = new Contract();

        // Act
        _cut.Add(" ", contract);

        // Assert
        _cut.Size(" ").Should().Be(1);
    }

    [Fact]
    public void AddRecent_DoesNotAddContractToQueue_WhenContractAlreadyInQueue()
    {
        // Arrange
        var contract = new Contract();

        // Act
        _cut.Add(" ", contract);
        _cut.Add(" ", contract);

        // Assert
        _cut.Size(" ").Should().Be(1);
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
        _cut.Add(" ", contract1);
        _cut.Add(" ", contract2);
        _cut.Add(" ", contract3);
        _cut.Add(" ", contract4);

        // Assert
        using (new AssertionScope())
        {
            _cut.Size(" ").Should().Be(3);
            ICollection<Contract> recentContracts = _cut.FetchRecentContracts(" ").ToList();
            recentContracts.Should().NotContain(contract1);
            recentContracts.Should().Contain(contract2);
            recentContracts.Should().Contain(contract3);
            recentContracts.Should().Contain(contract4);
        }
    }

    [Fact]
    public void RemoveContract_ShouldRemoveFromRecentlyViewed_WhenContractHasBeenViewed()
    {
        // Arrange
        var contract1 = new Contract();
        var contract2 = new Contract() { Name = "contract 2", };
        _cut.Add(" ", contract1);
        _cut.Add(" ", contract2);
        Guid id = contract2.Id;

        // Act
        _cut.Add(" ", contract1);
        _cut.Add(" ", contract2);
        _cut.Remove(id);

        // Assert
        _cut.FetchRecentContracts(" ").Count().Should().Be(1);
    }
}
