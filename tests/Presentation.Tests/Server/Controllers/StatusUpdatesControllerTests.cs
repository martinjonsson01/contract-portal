using Application.StatusUpdates;

using Domain.StatusUpdates;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Tests.Server.Controllers;

public class StatusUpdatesControllerTests
{
    private readonly StatusUpdatesController _cut;

    public StatusUpdatesControllerTests()
    {
        _cut = new StatusUpdatesController(Mock.Of<ILogger<StatusUpdatesController>>(), new NotificationService());
    }

    [Fact]
    public void Create_ReturnsOk_WhenValidStatusUpdateIsInput()
    {
        // Arrange
        var newUpdate = new StatusUpdate();

        // Act
        IActionResult response = _cut.Create(newUpdate);

        // Assert
        response.Should().BeAssignableTo<OkResult>();
    }
}
