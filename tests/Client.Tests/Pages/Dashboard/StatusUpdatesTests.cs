using Client.Pages.Dashboard;

using Domain.StatusUpdates;

namespace Client.Tests.Pages.Dashboard;

public class StatusUpdatesTests : UITestFixture
{
    public StatusUpdatesTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

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

        static void ParameterBuilder(ComponentParameterCollectionBuilder<StatusUpdates> parameters) =>
            parameters.Add(property => property.AlertLevels, Enum.GetValues(typeof(AlertLevel)).Cast<AlertLevel>());

        // Act
        IRenderedComponent<StatusUpdates> cut = Context.RenderComponent<StatusUpdates>(ParameterBuilder);
        cut.WaitForElement(notification);

        // Assert
        cut.WaitForAssertion(() => cut.FindAll(notification).Should().HaveCount(statusUpdates.Count));
    }

    [Fact]
    public void StatusUpdates_ThrowsArgumentOutOfRange_WhenAlertLevelIsNotDefined()
    {
        // Arrange
        var statusUpdates = new List<StatusUpdate> { new() { Alert = (AlertLevel)42, }, };
        MockHttp.When("/api/v1/status-updates").RespondJson(statusUpdates);

        const string notification = ".list-group-item";

        static void ParameterBuilder(ComponentParameterCollectionBuilder<StatusUpdates> parameters) =>
            parameters.Add(property => property.AlertLevels, new[] { (AlertLevel)42, });

        // Act
        IRenderedComponent<StatusUpdates> cut = Context.RenderComponent<StatusUpdates>(ParameterBuilder);
        Action waitForRender = () => cut.WaitForElement(notification);

        // Assert
        waitForRender.Should().Throw<ArgumentOutOfRangeException>();
    }
}
