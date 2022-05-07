using Application.Exceptions;
using Application.FavoriteContracts;
using Application.Users;

using Domain.Users;

using Microsoft.AspNetCore.Mvc;

namespace Server.Tests.Controllers;

public class FavoritesControllerTests
{
    private readonly FavoritesController _cut;
    private readonly Mock<IFavoriteContractService> _mockUsers;

    public FavoritesControllerTests()
    {
        _mockUsers = new Mock<IFavoriteContractService>();
        _cut = new FavoritesController(Mock.Of<ILogger<FavoritesController>>(), _mockUsers.Object);
    }

    [Fact]
    public void Gets_Favorites()
    {
        // Arrange
        List<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        _mockContracts.Setup(service => service.FetchFavorites()).Returns(fakeContracts);

        // Act
        IEnumerable<Contract> favorites = _cut.Favorites();

        // Assert
        favorites.Should().BeEquivalentTo(fakeContracts);
    }

    [Fact]
    public void UpdatesContract_ChangesContractFavoriteStatusCorrectly()
    {
        // Arrange
        var patchDocument = new JsonPatchDocument<Contract>();
        var contract = new Contract() { IsFavorite = true, };
        patchDocument.Replace(c => c.IsFavorite, !contract.IsFavorite);
        _mockContracts.Setup(service => service.FetchContract(contract.Id)).Returns(contract);

        // Act
        _cut.UpdateContract(patchDocument, contract.Id);

        // Assert
        contract.IsFavorite.Should().Be(false);
    }
}
