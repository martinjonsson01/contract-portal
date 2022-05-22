using System;

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

    [Fact]
    public void RemovingNotification_DoesReturnTrue_WhenANotificationExists()
    {
        // Arrange
        var statusUpdate = new StatusUpdate();
        _mockRepo.Setup(repository => repository.Remove(statusUpdate.Id)).Returns(true);

        // Act
        bool actual = _cut.Remove(statusUpdate.Id);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void RemovingNotification_DoesReturnFalse_WhenNoNotificationsExists()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<StatusUpdate>());

        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }
}
