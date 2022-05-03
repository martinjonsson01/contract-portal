using System.Net;

using Client.Shared;

namespace Presentation.Tests.Client.Shared;

public class FetchDataTests : UITestFixture
{
    [Fact]
    public void Render_RendersErrorMessage_WhenEndpointUnauthorized()
    {
        // Arrange
        const string endpointUrl = "/api/v1/example-endpoint";
        MockHttp.When(endpointUrl).Respond(HttpStatusCode.Unauthorized);

        const string loading = "<p>loading</p>";

        static void ParameterBuilder(ComponentParameterCollectionBuilder<FetchData<int>> parameters) =>
            parameters.Add(property => property.Url, endpointUrl)
                      .Add(property => property.LoadingIndicator, loading)
                      .Add(property => property.ChildContent, _ => string.Empty);

        // Act
        IRenderedComponent<FetchData<int>> cut = Context.RenderComponent<FetchData<int>>(ParameterBuilder);

        // Assert
        cut.Find("#error-message").Should().NotBeNull();
    }
}
