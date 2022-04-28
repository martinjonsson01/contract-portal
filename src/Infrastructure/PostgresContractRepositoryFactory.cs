using Infrastructure.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
/// Used at design-time by Entity Framework Core to generate the database migrations.
/// </summary>
public class PostgresContractRepositoryFactory : IDesignTimeDbContextFactory<EFContractRepository>
{
    /// <inheritdoc />
    public EFContractRepository CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EFContractRepository>();
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
        return new EFContractRepository(
            optionsBuilder.Options,
            new LoggerFactory().CreateLogger<EFContractRepository>());
    }
}
