using Application.Exceptions;
using Application.FavoriteContracts;
using Application.MessagePassing;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Server.Tests.Controllers;

public class FavoritesControllerTests
{
    private readonly FavoritesController _cut;
    private readonly Mock<IFavoriteContractService> _mockContracts;

    public FavoritesControllerTests()
    {
        _mockContracts = new Mock<IFavoriteContractService>();
        _cut = new FavoritesController(Mock.Of<ILogger<FavoritesController>>(), _mockContracts.Object);
    }

    [Fact]
    public void Gets_Favorites()
    {
        // Arrange
        string userName = "user";
        List<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        _mockContracts.Setup(service => service.FetchAllFavorites(userName)).Returns(fakeContracts);

        // Act
        IEnumerable<Contract> favorites = _cut.GetAll(userName);

        // Assert
        favorites.Should().BeEquivalentTo(fakeContracts);
    }

    [Fact]
    public void Adds_Favorite()
    {
        // Arrange
        string userName = "user";
        _mockContracts.Setup(service => service.FetchAllFavorites(userName));
        SetFavoriteContract setFavoriteContract = new SetFavoriteContract()
        {
            UserName = userName, ContractId = Guid.NewGuid(), IsFavorite = true,
        };

        // Act
        var result = _cut.Add(setFavoriteContract);

        // Assert
    }

    [Fact]
    public void UpdatesContract_ChangesContractFavoriteStatusCorrectly()
    {
        // Broken test moved over from contract controller
        // Arrange
        var patchDocument = new JsonPatchDocument<Contract>();
        var contract = new Contract();
        patchDocument.Replace(c => c.IsFavorite, !contract.IsFavorite);
        _mockContracts.Setup(service => service.FetchContract(contract.Id)).Returns(contract);

        // Act
        _cut.UpdateContract(patchDocument, contract.Id);

        // Assert
        contract.IsFavorite.Should().Be(false);
    }
}
