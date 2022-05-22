using Application.StatusUpdates;

using Domain.StatusUpdates;

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
    public void CreateCallsNotificationServiceOnce()
    {
        // Arrange
        var statusUpdate = new StatusUpdate();

        // Act
        _cut.Create(statusUpdate);

        // Assert
        _mockStatusUpdates.Verify(service => service.Add(statusUpdate), Times.Once);
    }
}
