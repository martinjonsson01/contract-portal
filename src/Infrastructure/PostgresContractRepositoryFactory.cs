using Infrastructure.Contracts;

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
        return new PostgresContractRepository(new LoggerFactory().CreateLogger<PostgresContractRepository>());
    }
}
