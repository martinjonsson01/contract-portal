using System;

using Application.Contracts;
using Application.Exceptions;
using Application.Search;

using Domain.Contracts;

namespace Application.Tests.Contracts;

public class ContractServiceTests
{
    private readonly ContractService _cut;
    private readonly Mock<IContractRepository> _mockRepo;

    public ContractServiceTests()
    {
        _mockRepo = new Mock<IContractRepository>();
        _cut = new ContractService(_mockRepo.Object, new SearchEngine<Contract>());
    }

    [Fact]
    public void FetchAllContracts_ReturnsAllContractsInTheDatabase()
    {
        // Arrange
        const int numberOfContracts = 10;
        List<Contract> mockContracts = new Faker<Contract>().Generate(numberOfContracts);
        _mockRepo.Setup(repository => repository.All).Returns(mockContracts);

        // Act
        IEnumerable<Contract> contracts = _cut.FetchAllContracts();

        // Assert
        contracts.Should().HaveCount(numberOfContracts);
    }

    [Fact]
    public void FetchRecentContracts_ReturnsOnlyRecentContractsFromTheDatabase()
    {
        // Arrange
        const int numberOfContracts = 3;
        List<Contract> mockContracts = new Faker<Contract>().Generate(numberOfContracts);
        _mockRepo.Setup(repository => repository.Recent).Returns(mockContracts);

        // Act
        IEnumerable<Contract> contracts = _cut.FetchRecentContracts();

        // Assert
        _mockRepo.Verify(repo => repo.Recent, Times.AtLeastOnce);
    }

    [Fact]
    public void AddingContract_ThrowsIDException_IfIDAlreadyTaken()
    {
        // Arrange
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.All).Returns(new[] { contract, });

        // Act
        Action add = () => _cut.Add(contract);

        // Assert
        add.Should().Throw<IdentifierAlreadyTakenException>();
    }

    [Fact]
    public void AddingContract_DoesNotThrow_IfIDIsUnique()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<Contract>());

        // Act
        Action add = () => _cut.Add(new Contract());

        // Assert
        add.Should().NotThrow();
    }

    [Fact]
    public void RemovingContract_DoesReturnTrue_WhenAContractExists()
    {
        // Arrange
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.Remove(contract.Id)).Returns(true);

        // Act
        bool actual = _cut.Remove(contract.Id);

        // // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void RemovingContract_DoesReturnFalse_WhenNoContractsExists()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<Contract>());

        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Searching_ReturnsAllContracts_WhenQueryIsEmpty()
    {
        // Arrange
        List<Contract> expected = new Faker<Contract>().Generate(10);
        _mockRepo.Setup(repository => repository.All).Returns(expected);

        // Act
        IEnumerable<Contract> actual = _cut.Search(string.Empty);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Searching_ReturnsNothing_WhenQueryIsGibberish()
    {
        // Arrange
        List<Contract> expected = new Faker<Contract>().Generate(10);
        _mockRepo.Setup(repository => repository.All).Returns(expected);

        // Act
        IEnumerable<Contract> actual = _cut.Search("blablablablablabla12987123981723xzxzcz");

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void FetchFavorite_CallsFavoriteFromRepoExactlyOnce()
    {
        // Act
        _cut.FetchFavorites();

        // Assert
        _mockRepo.Verify(repo => repo.Favorites, Times.Exactly(1));
    }

    [Fact]
    public void FetchingContract_ReturnsContractFromRepo_WhenContractExists()
    {
        // Arrange
        var expected = new Contract();
        _mockRepo.Setup(repository => repository.FetchContract(expected.Id)).Returns(expected);

        // Act
        Contract actual = _cut.FetchContract(expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void FetchingContract_ThrowsException_WhenContractDoesNotExist()
    {
        // Arrange

        // Act
        Action fetch = () => _cut.FetchContract(Guid.NewGuid());

        // Assert
        fetch.Should().Throw<ContractDoesNotExistException>();
    }
}
