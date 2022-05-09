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
    public void GetAll_ReturnsAllFavoriteContracts()
    {
        // Arrange
        string userName = "user";
        List<Contract> fakeFavoriteContracts = new Faker<Contract>().Generate(10);
        _mockContracts.Setup(service => service.FetchAll(userName)).Returns(fakeFavoriteContracts);

        // Act
        IEnumerable<Contract> favoriteContracts = _cut.GetAll(userName);

        // Assert
        favoriteContracts.Should().BeEquivalentTo(fakeFavoriteContracts);
    }

    [Fact]
    public void Change_ReturnsOk()
    {
        // Arrange
        var setFavoriteContract = new SetFavoriteContract()
        {
            UserName = "user", ContractId = Guid.NewGuid(), IsFavorite = true,
        };

        // Act
        IActionResult actual = _cut.Change(setFavoriteContract);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Change_CallsAddInService_IfTheContractShouldBeAFavorite()
    {
        // Arrange
        var setFavoriteContract = new SetFavoriteContract()
        {
            UserName = "user", ContractId = Guid.NewGuid(), IsFavorite = true,
        };

        // Act
        _cut.Change(setFavoriteContract);

        // Assert
        _mockContracts.Verify(service => service.Add(setFavoriteContract.UserName, setFavoriteContract.ContractId), Times.Once);
    }

    [Fact]
    public void Change_CallsRemoveInService_IfTheContractShouldNotBeAFavorite()
    {
        // Arrange
        var setFavoriteContract = new SetFavoriteContract()
        {
            UserName = "user", ContractId = Guid.NewGuid(), IsFavorite = false,
        };

        // Act
        _cut.Change(setFavoriteContract);

        // Assert
        _mockContracts.Verify(service => service.Remove(setFavoriteContract.UserName, setFavoriteContract.ContractId), Times.Once);
    }

    [Fact]
    public void GetIsFavorite_ReturnsOk_IfTheContractIsMarkedAsFavoriteByTheUser()
    {
        // Arrange
        string userName = "user";
        Guid contractId = Guid.NewGuid();
        _mockContracts.Setup(service => service.IsFavorite(userName, contractId)).Returns(true);

        // Act
        IActionResult actual = _cut.GetIsFavorite(userName, contractId);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void GetIsFavorite_ReturnsBadRequest_IfTheContractIsNotMarkedAsFavoriteByTheUser()
    {
        // Arrange
        string userName = "user";
        Guid contractId = Guid.NewGuid();
        _mockContracts.Setup(service => service.IsFavorite(userName, contractId)).Returns(false);

        // Act
        IActionResult actual = _cut.GetIsFavorite(userName, contractId);

        // Assert
        actual.Should().BeOfType<BadRequestResult>();
    }
}
