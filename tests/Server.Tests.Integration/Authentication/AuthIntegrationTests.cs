using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Configuration;
using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using FluentAssertions.Execution;

namespace Server.IntegrationTests.Authentication;

public class AuthIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        Environment.SetEnvironmentVariable(
            ConfigurationKeys.AdminPassword,
            "test_password",
            EnvironmentVariableTarget.Process);
    }

    public static IEnumerable<object[]> AdminPostApiEndpoints =>
        new List<object[]>
        {
            new object[] { "/api/v1/contracts", JsonContent.Create(new Contract()), },
            new object[] { "/api/v1/users", JsonContent.Create(new User()), },
        };

    public static IEnumerable<object[]> AdminDeleteApiEndpoints
    {
        get
        {
            const string contractEndpoint = "/api/v1/contracts";
            var contract = new Contract();
            Func<HttpClient, Task> createContract =
                async client => await client.PostAsJsonAsync(contractEndpoint, contract);

            const string usersEndpoint = "/api/v1/users";
            var user = new User();
            Func<HttpClient, Task> createUser = async client => await client.PostAsJsonAsync(usersEndpoint, user);

            return new List<object[]>
            {
                new object[] { contractEndpoint + $"/{contract.Id}", createContract, },
                new object[] { usersEndpoint + $"/{user.Id}", createUser, },
            };
        }
    }

    [Theory]
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
    [MemberData(nameof(AdminPostApiEndpoints))]
    public async Task SendToAdminPostApiEndpoints_ReturnsUnauthorized_WhenTokenIsNotSpecifiedAsync(
        string endpointUrl,
        HttpContent content)
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _client.PostAsync(endpointUrl, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(AdminPostApiEndpoints))]
    public async Task SendToAdminPostApiEndpoints_ReturnsForbidden_WhenUserTokenIsSpecifiedAsync(
        string endpointUrl,
        HttpContent content)
    {
        // Arrange
        await ArrangeAuthenticatedUser();

        // Act
        HttpResponseMessage response = await _client.PostAsync(endpointUrl, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [MemberData(nameof(AdminPostApiEndpoints))]
    public async Task SendToAdminPostApiEndpoints_IsSuccessful_WhenAdminIsAuthenticatedAsync(
        string endpointUrl,
        HttpContent content)
    {
        // Arrange
        await ArrangeAuthenticatedAdmin();

        // Act
        HttpResponseMessage response = await _client.PostAsync(endpointUrl, content);

        // Assert
        response.Should().BeSuccessful();
    }

    [Theory]
    [MemberData(nameof(AdminDeleteApiEndpoints))]
    public async Task SendToAdminDeleteApiEndpoints_ReturnsForbidden_WhenUserTokenIsSpecifiedAsync(
        string endpointUrl,
        Func<HttpClient, Task> createResource)
    {
        // Arrange
        await ArrangeAuthenticatedUser();
        await createResource(_client);

        // Act
        HttpResponseMessage response = await _client.DeleteAsync(endpointUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [MemberData(nameof(AdminDeleteApiEndpoints))]
    public async Task SendToAdminDeleteApiEndpoints_IsSuccessful_WhenAdminIsAuthenticatedAsync(
        string endpointUrl,
        Func<HttpClient, Task> createResource)
    {
        // Arrange
        await ArrangeAuthenticatedAdmin();
        await createResource(_client);

        // Act
        HttpResponseMessage response = await _client.DeleteAsync(endpointUrl);

        // Assert
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task GetContractsApiEndpoint_ReturnsPreviewContent_WhenUserIsNotAuthenticatedAsync()
    {
        // Arrange
        var contract = new Contract();
        await ArrangeResource("/api/v1/contracts", contract);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/v1/contracts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var contractPreviews = await response.Content.ReadFromJsonAsync<ICollection<ContractPreviewDto>>();
        contractPreviews.Should().NotBeNull();
        ContractPreviewDto preview = contractPreviews!.First(preview => preview.Id == contract.Id);

        using (new AssertionScope())
        {
            preview.Id.Should().Be(contract.Id);
            preview.Name.Should().Be(contract.Name);
            preview.Description.Should().Be(contract.Description);
            preview.SupplierLogoImagePath.Should().Be(contract.SupplierLogoImagePath);
            preview.InspirationalImagePath.Should().Be(contract.InspirationalImagePath);
        }
    }

    [Fact]
    public async Task GetContractsApiEndpoint_DoesNotReturnConfidentialContent_WhenUserIsNotAuthenticatedAsync()
    {
        // Arrange
        var contract = new Contract { Instructions = "very secret usage instructions", };
        await ArrangeResource("/api/v1/contracts", contract);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/v1/contracts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var contracts = await response.Content.ReadFromJsonAsync<IEnumerable<Contract>>();
        contracts.Should().NotContainEquivalentOf(contract);
    }

    [Fact]
    public async Task GetContractsApiEndpoint_ReturnsOkContent_WhenUserIsAuthenticatedAsync()
    {
        // Arrange
        var contract = new Contract();
        await ArrangeResource("/api/v1/contracts", contract);

        await ArrangeAuthenticatedUser();

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/v1/contracts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contracts = await response.Content.ReadFromJsonAsync<IEnumerable<Contract>>();
        contracts.Should().ContainEquivalentOf(contract);
    }

    private async Task ArrangeResource<TResource>(string url, TResource resource)
    {
        await ArrangeAuthenticatedAdmin();
        await _client.PostAsJsonAsync(url, resource);

        // Log out.
        _client.DefaultRequestHeaders.Authorization = null;
    }

    private async Task ArrangeAuthenticatedAdmin()
    {
        var userInfo = new User() { Name = "admin", Password = Environment.GetEnvironmentVariable(ConfigurationKeys.AdminPassword) ?? string.Empty, };
        HttpResponseMessage authResponseMessage = await _client.PostAsJsonAsync("/api/v1/users/authenticate", userInfo);
        AuthenticateResponse? authResponse =
            await authResponseMessage.Content.ReadFromJsonAsync<AuthenticateResponse>();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResponse?.Token);
    }

    private async Task ArrangeAuthenticatedUser()
    {
        // Arrange - authenticate as admin user.
        await ArrangeAuthenticatedAdmin();

        // Arrange - create normal user.
        var user = new User();
        await _client.PostAsJsonAsync("/api/v1/users", user);

        // Arrange - authenticate as normal user.
        HttpResponseMessage authResponseMessage =
            await _client.PostAsJsonAsync("/api/v1/users/authenticate", user);
        AuthenticateResponse? authResponse =
            await authResponseMessage.Content.ReadFromJsonAsync<AuthenticateResponse>();

        // Swap out the admin token for a normal user token.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResponse?.Token);
    }
}
