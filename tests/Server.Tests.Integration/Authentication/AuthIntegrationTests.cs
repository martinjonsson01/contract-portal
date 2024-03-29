﻿using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Application.Contracts;

using Domain.Contracts;
using Domain.Users;

using FluentAssertions.Execution;

namespace Server.IntegrationTests.Authentication;

public class AuthIntegrationTests : IntegrationTest
{
    public AuthIntegrationTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    public static IEnumerable<object[]> AdminPutApiEndpoints
    {
        get
        {
            return new List<object[]>
            {
                new object[] { "/api/v1/users", new User { Name = "Unique name", }, },
                new object[] { "/api/v1/contracts", new Contract(), },
            };
        }
    }

    public static IEnumerable<object[]> AdminDeleteApiEndpoints
    {
        get
        {
            var contract = new Contract();
            Func<HttpClient, Task> createContract =
                async client => await client.PutAsJsonAsync("/api/v1/contracts", contract);

            var user = new User { Name = "User Usersson", };
            Func<HttpClient, Task> createUser = async client => await client.PutAsJsonAsync("/api/v1/users", user);

            return new List<object[]>
            {
                new object[] { $"/api/v1/contracts/{contract.Id}", createContract, },
                new object[] { $"/api/v1/users/{user.Id}", createUser, },
            };
        }
    }

    [Theory]
    [InlineData("/api/v1/users")]
    public async Task GetApiEndpoints_ReturnsUnauthorized_WhenNoTokenIsSpecifiedAsync(string endpointUrl)
    {
        // Arrange

        // Act
        HttpResponseMessage response = await Client.GetAsync(endpointUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(AdminPutApiEndpoints))]
    public async Task SendToAdminPutApiEndpoints_ReturnsUnauthorized_WhenTokenIsNotSpecifiedAsync(
        string endpointUrl,
        object content)
    {
        // Arrange

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync(endpointUrl, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(AdminPutApiEndpoints))]
    public async Task SendToAdminPutApiEndpoints_ReturnsForbidden_WhenUserTokenIsSpecifiedAsync(
        string endpointUrl,
        object content)
    {
        // Arrange
        await ArrangeAuthenticatedUserAsync();

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync(endpointUrl, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [MemberData(nameof(AdminPutApiEndpoints))]
    public async Task SendToAdminPutApiEndpoints_IsSuccessful_WhenAdminIsAuthenticatedAsync(
        string endpointUrl,
        object content)
    {
        // Arrange
        await ArrangeAuthenticatedAdminAsync();

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync(endpointUrl, content);

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
        await ArrangeAuthenticatedUserAsync();
        await createResource(Client);

        // Act
        HttpResponseMessage response = await Client.DeleteAsync(endpointUrl);

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
        await ArrangeAuthenticatedAdminAsync();
        await createResource(Client);

        // Act
        HttpResponseMessage response = await Client.DeleteAsync(endpointUrl);

        // Assert
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task GetContractsApiEndpoint_ReturnsPreviewContent_WhenUserIsNotAuthenticatedAsync()
    {
        // Arrange
        var contract = new Contract();
        await PutResourceAsync($"/api/v1/contracts", contract);

        // Act
        HttpResponseMessage response = await Client.GetAsync("/api/v1/contracts");

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
        await PutResourceAsync("/api/v1/contracts", contract);

        // Act
        HttpResponseMessage response = await Client.GetAsync("/api/v1/contracts");

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
        await PutResourceAsync($"/api/v1/contracts", contract);

        await ArrangeAuthenticatedUserAsync();

        // Act
        HttpResponseMessage response = await Client.GetAsync("/api/v1/contracts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contracts = await response.Content.ReadFromJsonAsync<IEnumerable<Contract>>();
        contracts.Should().ContainEquivalentOf(contract);
    }
}
