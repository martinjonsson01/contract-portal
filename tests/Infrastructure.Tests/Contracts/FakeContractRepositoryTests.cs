using System;
using System.Linq;
using Domain.Contracts;
using Infrastructure.Contracts;

namespace Infrastructure.Tests.Contracts;

public class FakeContractRepositoryTests
{
    private readonly FakeContractRepository _cut;

    public FakeContractRepositoryTests()
    {
        _cut = new FakeContractRepository();
    }

    [Fact]
    public void RemoveContract_ReturnsFalse_WhenContractDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void RemoveContract_ReturnTrue_WhenContractExists()
    {
        // Arrange
        Guid id = _cut.All.First().Id;

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Favorites_ReturnCorrectAmountOfFavoriteContracts()
    {
        // Arrange

        // Act
        IEnumerable<Contract> favorites = _cut.Favorites();

        // Assert
        favorites.Should().ContainSingle();
    }
}
