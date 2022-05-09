using System;
using Application.FavoriteContracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;

namespace Application.Tests.Favorites;

public class FavoriteContractServiceTests
{
    private readonly FavoriteContractService _cut;
    private readonly Mock<IFavoriteContractRepository> _mockFavoriteRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;

    public FavoriteContractServiceTests()
    {
        _mockFavoriteRepo = new Mock<IFavoriteContractRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _cut = new FavoriteContractService(_mockFavoriteRepo.Object, _mockUserRepo.Object);
    }

    [Fact]
    public void FetchAll_CallsFetchAllFavoritesFromRepoExactlyOnce()
    {
        // Arrange
        List<Contract> contracts = new List<Contract>() { new Contract(), new Contract(), new Contract() };
        var user = new User() { Contracts = contracts };
        _mockUserRepo.Setup(repo => repo.Fetch(user.Name)).Returns(user);

        // Act
        IEnumerable<Contract> fetchedContracts = _cut.FetchAll(user.Name);

        // Assert
        fetchedContracts.Should().BeEquivalentTo(contracts);
    }

    [Fact]
    public void Add_CallsAddFromRepository()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();

        // Act
        _cut.Add(user.Name, contract.Id);

        // Assert
        _mockFavoriteRepo.Verify(repo => repo.Add(user.Name, contract.Id), Times.Once);
    }

    [Fact]
    public void IsFavorite_ReturnsTrue_IfContractIsMarkedAsFavorite()
    {
        // Arrange
        Contract contract = new Contract();
        List<Contract> contracts = new List<Contract>() { contract, new Contract(), new Contract() };
        var user = new User() { Contracts = contracts };
        _mockUserRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

        // Act
        bool actual = _cut.IsFavorite(user.Name, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void IsFavorite_ReturnsFalse_IfContractIsNotMarkedAsFavorite()
    {
        // Arrange
        Contract contract = new Contract();
        List<Contract> contracts = new List<Contract>() { new Contract(), new Contract(), new Contract() };
        var user = new User() { Contracts = contracts };
        _mockUserRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

        // Act
        bool actual = _cut.IsFavorite(user.Name, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void FetchAll_ReturnsAllFavoriteContracts()
    {
        // Arrange
        List<Contract> mockFavoriteContracts = new Faker<Contract>().Generate(5);
        var user = new User() { Contracts = mockFavoriteContracts };
        _mockUserRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

        // Act
        IEnumerable<Contract> favoriteContracts = _cut.FetchAll(user.Name);

        // Assert
        favoriteContracts.Should().BeEquivalentTo(mockFavoriteContracts);
    }

    [Fact]
    public void FetchAll_Throws_WhenTheUserDoesNotExist()
    {
        // Arrange
        List<Contract> mockFavoriteContracts = new Faker<Contract>().Generate(5);
        var user = new User() { Contracts = mockFavoriteContracts };
        _mockUserRepo.Setup(repository => repository.Fetch(user.Name)).Returns<User?>(null);

        // Act
        Action add = () => _cut.FetchAll(user.Name);

        // Assert
        add.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Remove_ReturnsTrue_IfTheRemovalWasSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockFavoriteRepo.Setup(repository => repository.Remove(user.Name, contract.Id)).Returns(true);

        // Act
        bool actual = _cut.Remove(user.Name, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Remove_ReturnsFalse_IfTheRemovalWasNotSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockFavoriteRepo.Setup(repository => repository.Remove(user.Name, contract.Id)).Returns(false);

        // Act
        bool actual = _cut.Remove(user.Name, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }
}
