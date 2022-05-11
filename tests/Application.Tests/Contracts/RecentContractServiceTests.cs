using System;
using System.Linq;
using Application.Contracts;
using Domain.Contracts;
using FluentAssertions.Execution;

namespace Application.Tests.Contracts;

public class RecentContractServiceTests
{
    private const string UserId = "123";
    private readonly IRecentContractService _cut;
    private readonly Mock<IRecentContractRepository> _mockRepo;

    public RecentContractServiceTests()
    {
        _mockRepo = new Mock<IRecentContractRepository>();
        _cut = new RecentContractService(_mockRepo.Object);
    }

    [Fact]
    public void Size_ReturnsTheExpectedAmount()
    {
        // Arrange
        var contract = new Contract();
        var contracts = new LinkedList<Contract>();
        contracts.AddFirst(contract);
        _mockRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        // Act
        int size = _cut.Size(UserId);

        // Assert
        size.Should().Be(1);
    }

    [Fact]
    public void AddRecent_DelegatesCorrectlyToRepository()
    {
        // Arrange
        var contract = new Contract();
        var contracts = new LinkedList<Contract>();
        contracts.AddFirst(contract);
        _mockRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        // Act
        _cut.Add(UserId, contract);

        // Assert
        _mockRepo.Verify(repo => repo.Add(UserId, contract), Times.Once);
    }

    [Fact]
    public void AddRecent_ShouldDequeueTheOldestContract_WhenMoreQueueCapacityIsAdded()
    {
        // Arrange
        var contract1 = new Contract();
        var contract2 = new Contract();
        var contract3 = new Contract();
        var contract4 = new Contract();
        var contracts = new LinkedList<Contract>();

        _mockRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);
        contracts.AddFirst(contract1);
        contracts.AddFirst(contract2);
        contracts.AddFirst(contract3);

        // Act
        _cut.Add(UserId, contract4);

        // Assert
        _mockRepo.Verify(repo => repo.RemoveLast(UserId), Times.Once);
    }
}
