using Application.Contracts;
using Domain.Contracts;

namespace Server.Tests.Controllers;

public class RecentsControllerTests
{
    private readonly RecentsController _cut;
    private readonly Mock<IRecentContractService> _mockRecent;

    public RecentsControllerTests()
    {
        _mockRecent = new Mock<IRecentContractService>();
        _cut = new RecentsController(
            Mock.Of<ILogger<RecentsController>>(),
            _mockRecent.Object);
    }

    [Fact]
    public void FetchRecent_ReturnsEmptyList_WhenUsernameIsEmpty()
    {
        // Act
        IEnumerable<Contract> recentContracts = _cut.RecentContracts(string.Empty);

        // Assert
        recentContracts.Should().BeEmpty();
    }

    [Fact]
    public void FetchRecent_DelegatesToService_WhenUsernameIsNotEmpty()
    {
        // Arrange
        const string username = "123";

        // Act
        _cut.RecentContracts(username);

        // Assert
        _mockRecent.Verify(recent => recent.FetchRecentContracts(username), Times.Once);
    }

    [Fact]
    public void AddRecent_DelegatesToService()
    {
        // Arrange
        const string username = "123";
        var contract = new Contract();

        // Act
        _cut.Add(username, contract);

        // Assert
        _mockRecent.Verify(recent => recent.Add(username, contract), Times.Once);
    }
}
