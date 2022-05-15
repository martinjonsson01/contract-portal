using System;
using System.Linq;
using Application.Contracts;
using Domain.Contracts;
using Domain.Users;
using FluentAssertions.Execution;

namespace Application.Tests.Contracts;

public class RecentContractServiceTests
{
    private const string UserName = "123";
    private readonly IRecentContractService _cut;
    private readonly Mock<IRecentContractRepository> _mockRecentRepo;
    private readonly Mock<IContractRepository> _mockContractRepo;

    public RecentContractServiceTests()
    {
        _mockRecentRepo = new Mock<IRecentContractRepository>();
        _mockContractRepo = new Mock<IContractRepository>();
        _cut = new LimitedRecentContractService(_mockRecentRepo.Object, _mockContractRepo.Object);
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
        int size = _cut.Size(UserName);

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
        _cut.Add(UserName, contract);

        // Assert
        _mockRecentRepo.Verify(repo => repo.Add(UserName, contract), Times.Once);
    }

    [Fact]
    public void AddRecent_ShouldDequeueTheOldestContract_WhenMoreQueueCapacityIsAdded()
    {
        // Arrange
        var contract1 = new Contract();
        var contracts = new List<RecentlyViewedContract>
        {
            new RecentlyViewedContract(),
            new RecentlyViewedContract(),
            new RecentlyViewedContract(),
            new RecentlyViewedContract(),
        };

        _mockRecentRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        // Act
        _cut.Add(UserName, contract1);

        // Assert
        _mockRecentRepo.Verify(repo => repo.Remove(It.IsAny<RecentlyViewedContract>()), Times.Once);
    }

    [Fact]
    public void AddRecent_ShouldNotDequeueTheOldestContract_WhenAddingAnAlreadyAddedContract()
    {
        // Arrange
        var contract1 = new Contract();
        var contract2 = new Contract();
        var contract3 = new Contract();
        var recentContract1 = new RecentlyViewedContract(new User().Id, contract1.Id);
        var recentContract2 = new RecentlyViewedContract(new User().Id, contract2.Id);
        var recentContract3 = new RecentlyViewedContract(new User().Id, contract3.Id);

        var contracts = new List<RecentlyViewedContract> { recentContract1, recentContract2, recentContract3, };

        _mockRecentRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        // Act
        _cut.Add(UserName, contract1);

        // Assert
        _mockRecentRepo.Verify(repo => repo.Remove(recentContract1), Times.Never);
    }

    [Fact]
    public void FetchRecentContracts_ShouldReturnSortedOrder_WhenGivenWrongOrder()
    {
        // Arrange
        var contract1 = new Contract();
        var contract2 = new Contract();
        var contract3 = new Contract();

        var recent1 = new RecentlyViewedContract(contract1.Id, new User().Id);
        var recent2 = new RecentlyViewedContract(contract2.Id, new User().Id);
        var recent3 = new RecentlyViewedContract(contract3.Id, new User().Id);

        var contracts = new List<RecentlyViewedContract> { recent2, recent3, recent1, };

        _mockRecentRepo.Setup(repo => repo.FetchRecentContracts(It.IsAny<string>()))
            .Returns(contracts);

        _mockContractRepo.Setup(repo => repo.FetchContract(contract1.Id))
            .Returns(contract1);
        _mockContractRepo.Setup(repo => repo.FetchContract(contract2.Id))
            .Returns(contract2);
        _mockContractRepo.Setup(repo => repo.FetchContract(contract3.Id))
            .Returns(contract3);

        // Act
        IEnumerable<Contract> recentContracts = _cut.FetchRecentContracts(UserName);

        // Assert
        recentContracts.SequenceEqual(new List<Contract>() { contract3, contract2, contract1 }).Should().BeTrue();
    }
}
