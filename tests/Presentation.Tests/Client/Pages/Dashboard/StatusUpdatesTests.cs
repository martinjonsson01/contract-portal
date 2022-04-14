using Client.Pages.Dashboard;

using Domain.StatusUpdates;

namespace Presentation.Tests.Client.Pages.Dashboard;

public class StatusUpdatesTests : UITestFixture
{
    [Fact]
    public void StatusUpdates_DisplaysFourNotifications_WhenServerReturnsFourStatusUpdates()
    {
        // Arrange
        var statusUpdates = new List<StatusUpdate>
        {
            new() { Alert = AlertLevel.Information, },
            new() { Alert = AlertLevel.Warning, },
            new() { Alert = AlertLevel.Urgent, },
            new() { Alert = AlertLevel.Critical, },
        };
        MockHttp.When("/api/v1/status-updates").RespondJson(statusUpdates);

        const string notification = ".list-group-item";

        // Act
        IRenderedComponent<StatusUpdates> cut = Context.RenderComponent<StatusUpdates>();
        cut.WaitForElement(notification);

        // Assert
        cut.FindAll(notification).Should().HaveCount(statusUpdates.Count);
    }

    [Fact]
    public void StatusUpdates_ThrowsArgumentOutOfRange_WhenAlertLevelIsNotDefined()
    {
        // Arrange
        var statusUpdates = new List<StatusUpdate>
        {
            new() { Alert = (AlertLevel)42, },
        };
        MockHttp.When("/api/v1/status-updates").RespondJson(statusUpdates);

        const string notification = ".list-group-item";

        // Act
        IRenderedComponent<StatusUpdates> cut = Context.RenderComponent<StatusUpdates>();
        Action waitForRender = () => cut.WaitForElement(notification);

        // Assert
        waitForRender.Should().Throw<ArgumentOutOfRangeException>();
    }
}
