using System.Diagnostics.CodeAnalysis;

using Application.Configuration;

using Infrastructure.Databases;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
/// Used at design-time by Entity Framework Core to generate the database migrations.
/// </summary>
[ExcludeFromCodeCoverage]
internal class EFDatabaseContextFactory : IDesignTimeDbContextFactory<EFDatabaseContext>
{
    /// <inheritdoc />
    public EFDatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EFDatabaseContext>();
        string? dbConnectionstring = Environment.GetEnvironmentVariable(ConfigurationKeys.DbConnectionString);

        if (dbConnectionstring == null)
        {
            throw new ArgumentException("No environment variable defined for " +
                                        ConfigurationKeys.DbConnectionString);
        }

        _ = optionsBuilder.UseSqlServer(dbConnectionstring);
        return new EFDatabaseContext(
            optionsBuilder.Options,
            new LoggerFactory().CreateLogger<EFDatabaseContext>());
    }
}
