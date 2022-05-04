using System.Diagnostics.CodeAnalysis;

using Infrastructure.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
/// Used at design-time by Entity Framework Core to generate the database migrations.
/// </summary>
[ExcludeFromCodeCoverage]
public class EFUserRepositoryFactory : IDesignTimeDbContextFactory<EFUserRepository>
{
    /// <inheritdoc />
    public EFUserRepository CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EFUserRepository>();
        string? dbConnectionstring = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.DbConnectionString);
        if (dbConnectionstring == null)
        {
            throw new ArgumentException("No environment variable defined for " +
                                        EnvironmentVariableKeys.DbConnectionString);
        }

        _ = optionsBuilder.UseSqlServer(dbConnectionstring);
        return new EFUserRepository(
            optionsBuilder.Options,
            new LoggerFactory().CreateLogger<EFUserRepository>());
    }
}
