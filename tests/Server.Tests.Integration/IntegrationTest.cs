﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Application.Configuration;
using Application.Users;

using Domain.Users;

namespace Server.IntegrationTests;

public class IntegrationTest : IClassFixture<TestWebApplicationFactory>
{
    public IntegrationTest(TestWebApplicationFactory factory)
    {
        Client = factory.CreateClient();

        Environment.SetEnvironmentVariable(
            ConfigurationKeys.AdminPassword,
            "test_password",
            EnvironmentVariableTarget.Process);
    }

    protected HttpClient Client { get; }

    protected async Task ArrangeResource<TResource>(string url, TResource resource)
    {
        await ArrangeAuthenticatedAdmin();
        await Client.PostAsJsonAsync(url, resource);

        // Log out.
        Client.DefaultRequestHeaders.Authorization = null;
    }

    protected async Task ArrangeAuthenticatedAdmin()
    {
        var userInfo = new User()
        {
            Name = "admin",
            Password = Environment.GetEnvironmentVariable(ConfigurationKeys.AdminPassword) ?? string.Empty,
        };
        HttpResponseMessage authResponseMessage = await Client.PostAsJsonAsync("/api/v1/users/authenticate", userInfo);
        AuthenticateResponse? authResponse =
            await authResponseMessage.Content.ReadFromJsonAsync<AuthenticateResponse>();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResponse?.Token);
    }

    protected async Task ArrangeAuthenticatedUser()
    {
        // Arrange - authenticate as admin user.
        await ArrangeAuthenticatedAdmin();

        // Arrange - create normal user.
        var user = new User();
        await Client.PostAsJsonAsync("/api/v1/users", user);

        // Arrange - authenticate as normal user.
        HttpResponseMessage authResponseMessage =
            await Client.PostAsJsonAsync("/api/v1/users/authenticate", user);
        AuthenticateResponse? authResponse =
            await authResponseMessage.Content.ReadFromJsonAsync<AuthenticateResponse>();

        // Swap out the admin token for a normal user token.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResponse?.Token);
    }
}
