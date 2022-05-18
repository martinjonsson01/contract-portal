using Application.StatusUpdates;

using Domain.StatusUpdates;
using Domain.Users;

using Microsoft.AspNetCore.Mvc;

namespace Server.Tests.Controllers;

public class StatusUpdatesControllerTests
{
    private readonly StatusUpdatesController _cut;
    private readonly Mock<IStatusUpdateService> _mockStatusUpdates;

    public StatusUpdatesControllerTests()
    {
        _mockStatusUpdates = new Mock<IStatusUpdateService>();
        _cut = new StatusUpdatesController(
            Mock.Of<ILogger<StatusUpdatesController>>(),
            _mockStatusUpdates.Object);
    }

    [Fact]
    public void Create_ReturnsOk()
    {
        // Arrange

        // Act
        IActionResult actual = _cut.Create(new StatusUpdate());

        // Assert
        actual.Should().BeOfType<OkResult>();
    }
}
