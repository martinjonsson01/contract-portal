using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Presentation.Tests.Server.Authentication;

public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    public static IEnumerable<object[]> AdminApiEndpoints =>
        new List<object[]>
        {
            new object[] { HttpMethod.Post, "/api/v1/contracts", },
            new object[] { HttpMethod.Post, "/api/v1/documents", },
            new object[] { HttpMethod.Post, "/api/v1/images", },
            new object[] { HttpMethod.Post, "/api/v1/users", },
        };

    [Theory]
    [InlineData("/api/v1/contracts")]
    [InlineData("/api/v1/users")]
    public async Task GetApiEndpoints_ReturnsUnauthorized_WhenNoTokenIsSpecifiedAsync(string endpointUrl)
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _client.GetAsync(endpointUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(AdminApiEndpoints))]
    public async Task SendToAdminApiEndpoints_ReturnsUnauthorized_WhenTokenForAdminIsNotSpecifiedAsync(
        HttpMethod method,
        string endpointUrl)
    {
        // Arrange
        var message = new HttpRequestMessage(method, endpointUrl);

        // Act
        HttpResponseMessage response = await _client.SendAsync(message);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
