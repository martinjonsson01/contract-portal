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
}
