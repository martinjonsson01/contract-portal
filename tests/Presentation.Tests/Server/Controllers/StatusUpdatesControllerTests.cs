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
    public void Resources_AreReturned()
    {
        // Act
        IEnumerable<StatusUpdate> actual = _cut.All();

        // Assert
        actual.Should().NotBeEmpty();
    }
}
