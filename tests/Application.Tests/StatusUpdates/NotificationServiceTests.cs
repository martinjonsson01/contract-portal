using Application.StatusUpdates;

using Domain.StatusUpdates;

namespace Application.Tests.StatusUpdates;

public class NotificationServiceTests
{
    private readonly NotificationService _cut;
    private readonly Mock<IStatusUpdateRepository> _mockRepo;

    public NotificationServiceTests()
    {
        _mockRepo = new Mock<IStatusUpdateRepository>();
        _cut = new NotificationService(_mockRepo.Object);
    }

    [Fact]
    public void AddingNotification_CallsRepoOnce()
    {
        // Arrange
        var statusUpdate = new StatusUpdate();

        // Act
        _cut.Add(statusUpdate);

        // Assert
        _mockRepo.Verify(repo => repo.Add(statusUpdate), Times.Once);
    }
}
