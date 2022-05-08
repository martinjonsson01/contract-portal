using Application.FavoriteContracts;

using Domain.Contracts;
using Domain.Users;

namespace Application.Tests.Favorites;

public class FavoriteContractServiceTests
{
    private readonly FavoriteContractService _cut;
    private readonly Mock<IFavoriteContractRepository> _mockRepo;

    public FavoriteContractServiceTests()
    {
        _mockRepo = new Mock<IFavoriteContractRepository>();
        _cut = new FavoriteContractService(_mockRepo.Object);
    }

    [Fact]
    public void FetchAllFavorites_CallsFetchAllFavoritesFromRepoExactlyOnce()
    {
        // Arrange
        var user = new User();

        // Act
        _cut.FetchAllFavorites(user.Name);

        // Assert
        _mockRepo.Verify(repo => repo.FetchAllFavorites(user.Name), Times.Exactly(1));
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
        _mockRepo.Verify(repo => repo.Add(user.Name, contract.Id), Times.Once);
    }

    [Fact]
    public void CheckIfFavorite_ReturnsTrue_IfContractIsMarkedAsFavorite()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.CheckIfFavorite(user.Name, contract.Id)).Returns(true);

        // Act
        bool actual = _cut.CheckIfFavorite(user.Name, contract.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void CheckIfFavorite_ReturnsFalse_IfContractIsNotMarkedAsFavorite()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.CheckIfFavorite(user.Name, contract.Id)).Returns(false);

        // Act
        bool actual = _cut.CheckIfFavorite(user.Name, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void FetchAllFavorites_ReturnsAllFavoriteContracts()
    {
        // Arrange
        var user = new User();
        List<Contract> mockFavoriteContracts = new Faker<Contract>().Generate(5);
        _mockRepo.Setup(repository => repository.FetchAllFavorites(user.Name)).Returns(mockFavoriteContracts);

        // Act
        IEnumerable<Contract> favoriteContracts = _cut.FetchAllFavorites(user.Name);

        // Assert
        favoriteContracts.Should().BeEquivalentTo(mockFavoriteContracts);
    }

    [Fact]
    public void Remove_ReturnsTrue_IfTheRemovalWasSuccessful()
    {
        // Arrange
        var user = new User();
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.Remove(user.Name, contract.Id)).Returns(true);

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
        _mockRepo.Setup(repository => repository.Remove(user.Name, contract.Id)).Returns(false);

        // Act
        bool actual = _cut.Remove(user.Name, contract.Id);

        // Assert
        actual.Should().BeFalse();
    }
}
