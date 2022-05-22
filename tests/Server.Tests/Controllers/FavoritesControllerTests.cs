using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;

using Microsoft.AspNetCore.Mvc;

namespace Server.Tests.Controllers;

public class FavoritesControllerTests
{
    private readonly FavoritesController _cut;
    private readonly Mock<IUserService> _mockContracts;

    public FavoritesControllerTests()
    {
        _mockContracts = new Mock<IUserService>();
        _cut = new FavoritesController(Mock.Of<ILogger<FavoritesController>>(), _mockContracts.Object);
    }

    [Fact]
    public void GetAll_ReturnsAllFavoriteContracts()
    {
        // Arrange
        var user = new User();
        List<Contract> fakeFavoriteContracts = new Faker<Contract>().Generate(10);
        _mockContracts.Setup(service => service.FetchAllFavorites(user.Id)).Returns(fakeFavoriteContracts);

        // Act
        IEnumerable<Contract> favoriteContracts = _cut.GetAll(user.Id);

        // Assert
        favoriteContracts.Should().BeEquivalentTo(fakeFavoriteContracts);
    }

    [Fact]
    public void Change_ReturnsOk_WhenContractAndUserExists()
    {
        // Arrange
        var favoriteContractDto = new FavoriteContractDto()
        {
            UserId = Guid.NewGuid(), ContractId = Guid.NewGuid(), IsFavorite = true,
        };

        // Act
        IActionResult actual = _cut.Change(favoriteContractDto);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Change_ReturnsBadRequest_IfUserDoesNotExist()
    {
        // Arrange
        var favoriteContractDto = new FavoriteContractDto()
        {
            UserId = Guid.NewGuid(), ContractId = Guid.NewGuid(), IsFavorite = true,
        };
        _mockContracts.Setup(service => service.AddFavorite(favoriteContractDto.UserId, favoriteContractDto.ContractId))
            .Throws<UserDoesNotExistException>();

        // Act
        IActionResult actual = _cut.Change(favoriteContractDto);

        // Assert
        actual.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Change_ReturnsBadRequest_IfContractDoesNotExist()
    {
        // Arrange
        var favoriteContractDto = new FavoriteContractDto()
        {
            UserId = Guid.NewGuid(), ContractId = Guid.NewGuid(), IsFavorite = true,
        };
        _mockContracts.Setup(service => service.AddFavorite(favoriteContractDto.UserId, favoriteContractDto.ContractId))
            .Throws<ContractDoesNotExistException>();

        // Act
        IActionResult actual = _cut.Change(favoriteContractDto);

        // Assert
        actual.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Change_CallsAddInService_IfTheContractShouldBeAFavorite()
    {
        // Arrange
        var favoriteContractDto = new FavoriteContractDto()
        {
            UserId = Guid.NewGuid(), ContractId = Guid.NewGuid(), IsFavorite = true,
        };

        // Act
        _cut.Change(favoriteContractDto);

        // Assert
        _mockContracts.Verify(service => service.AddFavorite(favoriteContractDto.UserId, favoriteContractDto.ContractId), Times.Once);
    }

    [Fact]
    public void Change_CallsRemoveInService_IfTheContractShouldNotBeAFavorite()
    {
        // Arrange
        var favoriteContractDto = new FavoriteContractDto()
        {
            UserId = Guid.NewGuid(), ContractId = Guid.NewGuid(), IsFavorite = false,
        };

        // Act
        _cut.Change(favoriteContractDto);

        // Assert
        _mockContracts.Verify(service => service.RemoveFavorite(favoriteContractDto.UserId, favoriteContractDto.ContractId), Times.Once);
    }

    [Fact]
    public void GetIsFavorite_ReturnsOk_IfTheContractIsMarkedAsFavoriteByTheUser()
    {
        // Arrange
        var user = new User();
        var contractId = Guid.NewGuid();
        _mockContracts.Setup(service => service.IsFavorite(user.Id, contractId)).Returns(true);

        // Act
        IActionResult actual = _cut.GetIsFavorite(user.Id, contractId);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void GetIsFavorite_ReturnsNotFound_IfTheContractIsNotMarkedAsFavoriteByTheUser()
    {
        // Arrange
        var user = new User();
        var contractId = Guid.NewGuid();
        _mockContracts.Setup(service => service.IsFavorite(user.Id, contractId)).Returns(false);

        // Act
        IActionResult actual = _cut.GetIsFavorite(user.Id, contractId);

        // Assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetIsFavorite_ReturnsNotFound_IfTheContractIsNotMarkedAsFavorite()
    {
        // Arrange
        var user = new User();
        var contractId = Guid.NewGuid();
        _mockContracts.Setup(service => service.IsFavorite(user.Id, contractId)).Returns(false);

        // Act
        IActionResult actual = _cut.GetIsFavorite(user.Id, contractId);

        // Assert
        actual.Should().BeOfType<NotFoundResult>();
    }
}
