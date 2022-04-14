using Client.Pages.Dashboard;

using Domain.StatusUpdates;

namespace Presentation.Tests.Client.Pages.Dashboard;

public class StatusUpdatesTests : UITestFixture
{
    [Fact]
    public void StatusUpdates_DisplaysNNotification_WhenServerReturnsNStatusUpdates()
    {
        // Arrange
        const int n = 3;
        List<StatusUpdate> statusUpdates = new Faker<StatusUpdate>().Generate(n);
        MockHttp.When("/api/v1/status-updates").RespondJson(statusUpdates);

        const string notification = ".list-group-item";

        // Act
        IRenderedComponent<StatusUpdates> cut = Context.RenderComponent<StatusUpdates>();
        cut.WaitForElement(notification);

        // Assert
        cut.FindAll(notification).Should().HaveCount(n);
    }
}
