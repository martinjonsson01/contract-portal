using Application.StatusUpdates;

using Domain.StatusUpdates;

using Infrastructure.StatusUpdates;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Tests.Server.Controllers;

public class StatusUpdatesControllerTests
{
    private readonly StatusUpdatesController _cut;
    private readonly InMemoryStatusUpdateRepository _repository;

    public StatusUpdatesControllerTests()
    {
        _repository = new InMemoryStatusUpdateRepository();
        _cut = new StatusUpdatesController(
            Mock.Of<ILogger<StatusUpdatesController>>(),
            new NotificationService(_repository));
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

    [Fact]
    public void Resource_IsReturned_WhenPreviouslyCreated()
    {
        // Arrange
        var expected = new StatusUpdate();
        _ = _cut.Create(expected);

        // Act
        StatusUpdate actual = _cut.All().First();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
