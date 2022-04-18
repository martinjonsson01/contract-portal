using Application.Contracts;

using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Tests.Server.Controllers;

public class ContractsControllerTests
{
    private readonly ContractsController _cut;
    private readonly Mock<IContractService> _mockContracts;

    public ContractsControllerTests()
    {
        _mockContracts = new Mock<IContractService>();
        _cut = new ContractsController(Mock.Of<ILogger<ContractsController>>(), _mockContracts.Object);
    }

    [Fact]
    public void Get_ReturnsAll_WhenQueryIsNull()
    {
        // Arrange
        List<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        _mockContracts.Setup(service => service.Search(string.Empty)).Returns(fakeContracts);

        // Act
        IEnumerable<Contract> actualWeather = _cut.Search(null);

        // Assert
        actualWeather.Should().BeEquivalentTo(fakeContracts);
    }

    [Fact]
    public void Get_ReturnsSearchResults_WhenQueryIsSet()
    {
        // Arrange
        List<Contract> searchResults = new Faker<Contract>().Generate(10);
        const string searchQuery = "keyword1 keyword2";
        _mockContracts.Setup(service => service.Search(searchQuery)).Returns(searchResults);

        // Act
        IEnumerable<Contract> actualWeather = _cut.Search(searchQuery);

        // Assert
        actualWeather.Should().BeEquivalentTo(searchResults);
    }

    [Fact]
    public void Remove_ReturnsOk_WhenIDExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockContracts.Setup(service => service.Remove(id)).Returns(true);

        // Act
        IActionResult actual = _cut.Remove(id);

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Remove_CallsContractService_WhenIDExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockContracts.Setup(service => service.Remove(id)).Returns(true);

        // Act
        IActionResult actual = _cut.Remove(id);

        // Assert
        _mockContracts.Verify(o => o.Remove(id), Times.Once);
    }

    [Fact]
    public void Remove_ReturnsNotFound_WhenIDDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockContracts.Setup(service => service.Remove(id)).Returns(false);

        // Act
        IActionResult actual = _cut.Remove(id);

        // Assert
        actual.Should().BeOfType<NotFoundResult>();
    }
}
