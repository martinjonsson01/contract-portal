using Infrastructure.Users;

using Microsoft.EntityFrameworkCore;
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
        var optionsBuilder = new DbContextOptionsBuilder<PostgresUserRepository>();
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
        return new PostgresUserRepository(
            optionsBuilder.Options,
            new LoggerFactory().CreateLogger<PostgresUserRepository>());
    }
}
