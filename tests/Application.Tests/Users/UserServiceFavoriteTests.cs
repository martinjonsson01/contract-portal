using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

using Application.Configuration;
using Application.Exceptions;
using Application.Users;
using Domain.Contracts;
using Domain.Users;

using FluentAssertions.Execution;

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
    public void Should_Fail()
    {
        false.Should().BeTrue();
    }

    [Fact]
    public void FetchAll_CallsFetchAllFavoritesFromRepoExactlyOnce()
    {
        // Arrange
        List<Contract> contracts = new List<Contract>() { new Contract(), new Contract(), new Contract() };
        var user = new User() { Favorites = contracts };
        _mockRepo.Setup(repo => repo.Fetch(user.Name)).Returns(user);

        // Act
        IEnumerable<Contract> fetchedContracts = _cut.FetchAllFavorites(user.Name);

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
        _cut.AddFavorite(user.Name, contract.Id);

        // Assert
        _mockRepo.Verify(repo => repo.AddFavorite(user.Name, contract.Id), Times.Once);
    }

    [Fact]
    public void IsFavorite_ReturnsTrue_IfContractIsMarkedAsFavorite()
    {
        // Arrange
        Contract contract = new Contract();
        List<Contract> contracts = new List<Contract>() { contract, new Contract(), new Contract() };
        var user = new User() { Favorites = contracts };
        _mockRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

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
        var user = new User() { Favorites = contracts };
        _mockRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

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
        var user = new User() { Favorites = mockFavoriteContracts };
        _mockRepo.Setup(repository => repository.Fetch(user.Name)).Returns(user);

        // Act
        IEnumerable<Contract> favoriteContracts = _cut.FetchAllFavorites(user.Name);

        // Assert
        favoriteContracts.Should().BeEquivalentTo(mockFavoriteContracts);
    }

    [Fact]
    public void FetchAll_Throws_WhenTheUserDoesNotExist()
    {
        // Arrange
        List<Contract> mockFavoriteContracts = new Faker<Contract>().Generate(5);
        var user = new User() { Favorites = mockFavoriteContracts };
        _mockRepo.Setup(repository => repository.Fetch(user.Name)).Returns<User?>(null);

        // Act
        Action add = () => _cut.FetchAllFavorites(user.Name);

        // Assert
        add.Should().Throw<UserDoesNotExistException>();
    }

    [Fact]
    public void Remove_ReturnsTrue_IfTheRemovalWasSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.RemoveFavorite(user.Name, contract.Id)).Returns(true);

        // Act
        bool actual = _cut.RemoveFavorite(user.Name, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Remove_ReturnsFalse_IfTheRemovalWasNotSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.RemoveFavorite(user.Name, contract.Id)).Returns(false);

        // Act
        bool actual = _cut.RemoveFavorite(user.Name, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }
}
