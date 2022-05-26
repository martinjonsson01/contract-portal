using System;

using Application.Configuration;
using Application.Users;

using Domain.Contracts;
using Domain.Users;

using Microsoft.Extensions.Configuration;

namespace Application.Tests.Users;

public class UserServiceFavoriteTests
{
    private readonly UserService _cut;
    private readonly Mock<IUserRepository> _mockRepo;

    public UserServiceFavoriteTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        var mockEnvironment = new Mock<IConfiguration>();
        mockEnvironment.Setup(env => env[ConfigurationKeys.JwtSecret]).Returns("test-json-web-token-secret");
        _cut = new UserService(_mockRepo.Object, mockEnvironment.Object);
    }

    [Fact]
    public void FetchAll_CallsFetchAllFavoritesFromRepoExactlyOnce()
    {
        // Arrange
        var contracts = new List<Contract> { new(), new(), new(), };
        var user = new User { Favorites = contracts, };
        _mockRepo.Setup(repo => repo.Fetch(user.Id)).Returns(user);

        // Act
        IEnumerable<Contract> fetchedContracts = _cut.FetchAllFavorites(user.Id);

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
        _cut.AddFavorite(user.Id, contract.Id);

        // Assert
        _mockRepo.Verify(repo => repo.AddFavorite(user.Id, contract.Id), Times.Once);
    }

    [Fact]
    public void IsFavorite_ReturnsTrue_IfContractIsMarkedAsFavorite()
    {
        // Arrange
        var contract = new Contract();
        var contracts = new List<Contract> { contract, new(), new(), };
        var user = new User { Favorites = contracts, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);

        // Act
        bool actual = _cut.IsFavorite(user.Id, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void IsFavorite_ReturnsFalse_IfContractIsNotMarkedAsFavorite()
    {
        // Arrange
        var contract = new Contract();
        var contracts = new List<Contract> { new(), new(), new(), };
        var user = new User { Favorites = contracts, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);

        // Act
        bool actual = _cut.IsFavorite(user.Id, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void FetchAll_ReturnsAllFavoriteContracts()
    {
        // Arrange
        List<Contract> mockFavoriteContracts = new Faker<Contract>().Generate(5);
        var user = new User { Favorites = mockFavoriteContracts, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns(user);

        // Act
        IEnumerable<Contract> favoriteContracts = _cut.FetchAllFavorites(user.Id);

        // Assert
        favoriteContracts.Should().BeEquivalentTo(mockFavoriteContracts);
    }

    [Fact]
    public void FetchAll_Throws_WhenTheUserDoesNotExist()
    {
        // Arrange
        List<Contract> mockFavoriteContracts = new Faker<Contract>().Generate(5);
        var user = new User { Favorites = mockFavoriteContracts, };
        _mockRepo.Setup(repository => repository.Fetch(user.Id)).Returns<User?>(null);

        // Act
        Action add = () => _cut.FetchAllFavorites(user.Id);

        // Assert
        add.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Remove_ReturnsTrue_IfTheRemovalWasSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.RemoveFavorite(user.Id, contract.Id)).Returns(true);

        // Act
        bool actual = _cut.RemoveFavorite(user.Id, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Remove_ReturnsFalse_IfTheRemovalWasNotSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.RemoveFavorite(user.Id, contract.Id)).Returns(false);

        // Act
        bool actual = _cut.RemoveFavorite(user.Id, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }
}
