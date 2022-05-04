using System.Diagnostics.CodeAnalysis;
using Application.Contracts;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Moq;

namespace Infrastructure;

/// <summary>
/// Used at design-time by Entity Framework Core to generate the database migrations.
/// </summary>
[ExcludeFromCodeCoverage]
public class EFContractRepositoryFactory : IDesignTimeDbContextFactory<EFContractRepository>
{
    /// <exception cref="ArgumentException">When environment variable does not exist.</exception>
    /// <inheritdoc />
    public EFContractRepository CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EFContractRepository>();
        string? dbConnectionstring = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.DbConnectionString);
        if (dbConnectionstring == null)
        {
            throw new ArgumentException("No environment variable defined for " +
                                        EnvironmentVariableKeys.DbConnectionString);
        }

        _ = optionsBuilder.UseSqlServer(dbConnectionstring);
        return new EFContractRepository(
            optionsBuilder.Options,
            Mock.Of<ILogger<EFContractRepository>>(),
            Mock.Of<IRecentContractService>());
    }
}
