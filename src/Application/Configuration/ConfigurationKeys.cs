﻿namespace Application.Configuration;

/// <summary>
/// The keys for every configurable variable of the application.
/// </summary>
public static class ConfigurationKeys
{
    /// <summary>
    /// The name of the variable containing the database connection string.
    /// </summary>
    public const string DbConnectionString = "prodigo_portal_db_connectionstring";

    /// <summary>
    /// The name of the variable containing the JWT secret.
    /// </summary>
    public const string JwtSecret = "prodigo_portal_jwt_secret";

    /// <summary>
    /// The name of the variable containing the JWT issuer domain.
    /// </summary>
    public const string JwtIssuer = "prodigo_portal_jwt_issuer";

    /// <summary>
    /// The name of the variable containing whether the system is running inside a CI-container.
    /// </summary>
    public const string ContinuousIntegration = "CONTINUOUS_INTEGRATION";

    /// <summary>
    /// The name of the variable containing the default admin password.
    /// </summary>
    public const string AdminPassword = "prodigo_portal_admin_password";
}
