using Application.Configuration;

namespace Infrastructure.Configuration;

/// <summary>
/// Gets configuration variables from the environment variables of the system.
/// </summary>
public class EnvironmentVariableConfiguration : IEnvironmentConfiguration
{
    /// <summary>
    /// Reads all environment variables and ensures they have values.
    /// </summary>
    public EnvironmentVariableConfiguration()
    {
        JwtSecret = GetEnvironmentVariable(EnvironmentVariableKeys.JwtSecret);
    }

    /// <inheritdoc/>
    public string JwtSecret { get; }

    private static string GetEnvironmentVariable(string key)
    {
        string? value = Environment.GetEnvironmentVariable(key);
        return value ?? throw new ArgumentException($"No environment variable defined for {key}");
    }
}
