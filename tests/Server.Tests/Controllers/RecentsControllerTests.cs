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
    public void FetchRecent_ReturnsEmptyList_WhenUserIdIsNull()
    {
        // Act
        IEnumerable<Contract> recentContracts = _cut.RecentContracts(null);

        // Assert
        recentContracts.Should().BeEmpty();
    }

    [Fact]
    public void FetchRecent_DelegatesToService_WhenUserIdIsGiven()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        _cut.RecentContracts(userId);

        // Assert
        _mockRecent.Verify(recent => recent.FetchRecentContracts(userId), Times.Once);
    }

    [Fact]
    public void AddRecent_DelegatesToService()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var contract = new Contract();

        // Act
        _cut.Add(userId, contract);

        // Assert
        _mockRecent.Verify(recent => recent.Add(userId, contract), Times.Once);
    }
}
