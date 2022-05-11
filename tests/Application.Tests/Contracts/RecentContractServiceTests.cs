using System;
using System.Linq;
using Application.Contracts;
using Domain.Contracts;
using Domain.Users;
using FluentAssertions.Execution;

namespace Application.Tests.Contracts;

public class RecentContractServiceTests
{
    private const string UserId = "123";
    private readonly IRecentContractService _cut;
    private readonly Mock<IRecentContractRepository> _mockRecentRepo;
    private readonly Mock<IContractRepository> _mockContractRepo;

    public RecentContractServiceTests()
    {
        _mockRecentRepo = new Mock<IRecentContractRepository>();
        _mockContractRepo = new Mock<IContractRepository>();
        _cut = new RecentContractService(_mockRecentRepo.Object, _mockContractRepo.Object);
    }

    [Fact]
    public void Size_ReturnsTheExpectedAmount()
    {
        // Arrange
        var contract = new Contract();
        var contracts = new List<RecentlyViewedContract> { new(contract.Id, new User().Id) };
        _mockRecentRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
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
        var contracts = new List<RecentlyViewedContract> { new RecentlyViewedContract() };
        _mockRecentRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        // Act
        _cut.Add(UserId, contract);

        // Assert
        _mockRecentRepo.Verify(repo => repo.AddRecent(UserId, contract), Times.Once);
    }

    [Fact]
    public void AddRecent_ShouldDequeueTheOldestContract_WhenMoreQueueCapacityIsAdded()
    {
        // Arrange
        var contract1 = new Contract();
        var contract2 = new Contract();
        var contract3 = new Contract();
        var contract4 = new Contract();
        var contracts = new List<RecentlyViewedContract> { new(contract1.Id, new User().Id), new(contract2.Id, new User().Id), new(contract3.Id, new User().Id) };

        _mockRecentRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        // Act
        _cut.Add(UserId, contract4);

        // Assert
        _mockRecentRepo.Verify(repo => repo.RemoveRecent(It.IsAny<RecentlyViewedContract>()), Times.Once);
    }
}
