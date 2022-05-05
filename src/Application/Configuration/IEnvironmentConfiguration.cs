namespace Application.Configuration;

/// <summary>
/// Configuration values for the environment that the application is running in.
/// </summary>
public interface IEnvironmentConfiguration
{
    /// <summary>
    /// Gets the JWT secret used to sign token keys.
    /// </summary>
    public string JwtSecret { get; }
}
