namespace Infrastructure;

/// <summary>
/// All environment variable keys.
/// </summary>
public static class EnvironmentVariableKeys
{
    /// <summary>
    /// The name of the environment variable containing the database connection string.
    /// </summary>
    public const string DbConnectionString = "prodigo_portal_db_connectionstring";

    /// <summary>
    /// The name of the environment variable containing whether the database connection is untrusted.
    /// </summary>
    public const string UntrustedConnection = "UNTRUSTED_CONNECTION";
}
