using Application.StatusUpdates;

using Domain.StatusUpdates;

using Infrastructure.StatusUpdates;

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
    public void Resource_IsReturned_WhenPreviouslyCreated()
    {
        // Arrange
        var expected = new StatusUpdate();

        // _ = _cut.Create(expected); Does not exist yet.

        // Act
        StatusUpdate actual = _cut.All().First();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
