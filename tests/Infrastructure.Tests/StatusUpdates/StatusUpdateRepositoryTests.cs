using System;

using Application.StatusUpdates;

using Domain.StatusUpdates;

using Infrastructure.Databases;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.StatusUpdates;

public class StatusUpdateRepositoryTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private IStatusUpdateRepository _cut;

    public StatusUpdateRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        EFDatabaseContext context = _fixture.CreateContext();
        _cut = new EFStatusUpdatesRepository(context, Mock.Of<ILogger<EFUserRepository>>());
        context.Database.BeginTransaction();
    }

    [Fact]
    public void AllStatusUpdates_ReturnsStatusUpdate_WhenItHasBeenAdded()
    {
        // Arrange
        var statusUpdate = new StatusUpdate();
        _cut.Add(statusUpdate);

        // Act
        IEnumerable<StatusUpdate> statusUpdates = _cut.All;

        // Assert
        statusUpdates.Should().NotBeNull();
    }

    [Fact]
    public void RemoveNotification_ReturnsFalse_WhenNotificationDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void RemoveNotification_ReturnsTrue_WhenNotificationDoesExist()
    {
        // Arrange
        StatusUpdate statusUpdate = new();
        _cut.Add(statusUpdate);

        // Act
        bool actual = _cut.Remove(statusUpdate.Id);

        // Assert
        actual.Should().BeTrue();
    }
}
