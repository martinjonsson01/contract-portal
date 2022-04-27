using Infrastructure.Users;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
/// Used at design-time by Entity Framework Core to generate the database migrations.
/// </summary>
public class PostgresUserRepositoryFactory : IDesignTimeDbContextFactory<PostgresUserRepository>
{
    /// <inheritdoc />
    public PostgresUserRepository CreateDbContext(string[] args)
    {
        return new PostgresUserRepository(new LoggerFactory().CreateLogger<PostgresUserRepository>());
    }
}
