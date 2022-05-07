using Application.Configuration;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Server.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestWebApplicationFactory()
    {
        string connectionString =
            @"Server=localhost;Database=ProdigoPortal_test;User Id=SA; Password=ASDjk_shd$$jkASKJ19821;";

        string? ciContainer = Environment.GetEnvironmentVariable(ConfigurationKeys.ContinuousIntegration);
        if (ciContainer is null)
            connectionString += "Trusted_Connection=True;";

        Environment.SetEnvironmentVariable(
            ConfigurationKeys.DbConnectionString,
            connectionString,
            EnvironmentVariableTarget.Process);

        Environment.SetEnvironmentVariable(
            ConfigurationKeys.JwtSecret,
            "test-json-web-token-secret",
            EnvironmentVariableTarget.Process);
    }
}
