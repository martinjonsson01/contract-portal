using System.Net;

using Client.Shared;

namespace Client.Tests.Shared;

public class FetchDataTests : UITestFixture
{
    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    public void Render_RendersErrorMessage_WhenEndpointUnauthorized(HttpStatusCode errorCode)
    {
        // Arrange
        MockSession.Setup(session => session.IsAuthenticated).Returns(false);

        const string endpointUrl = "/api/v1/example-endpoint";
        MockHttp.When(endpointUrl).Respond(errorCode);

        const string loading = "<p>loading</p>";

        static void ParameterBuilder(ComponentParameterCollectionBuilder<FetchData<int?>> parameters) =>
            parameters.Add(property => property.Url, endpointUrl)
                      .Add(property => property.LoadingIndicator, loading)
                      .Add(property => property.ChildContent, _ => string.Empty);

        // Act
        IRenderedComponent<FetchData<int?>> cut = Context.RenderComponent<FetchData<int?>>(ParameterBuilder);
        cut.WaitForElement("#error-message");

        // Assert
        cut.Markup.Should().Contain("error-message");
    }
}
