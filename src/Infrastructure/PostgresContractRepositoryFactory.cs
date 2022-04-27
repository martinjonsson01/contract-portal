using Infrastructure.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
/// Used at design-time by Entity Framework Core to generate the database migrations.
/// </summary>
public class PostgresContractRepositoryFactory : IDesignTimeDbContextFactory<PostgresContractRepository>
{
    /// <inheritdoc />
    public PostgresContractRepository CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContractRepository>();
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
        return new PostgresContractRepository(
            optionsBuilder.Options,
            new LoggerFactory().CreateLogger<PostgresContractRepository>());
    }
}
