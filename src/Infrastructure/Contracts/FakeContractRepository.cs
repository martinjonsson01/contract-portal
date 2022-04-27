using System.Collections.ObjectModel;
using System.Data;

using Application.Contracts;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Contracts;

/// <summary>
/// Mocks fake contracts.
/// </summary>
public class FakeContractRepository : DbContext, IContractRepository
{
    private readonly ILogger<FakeContractRepository> _logger;
    private readonly IRecentContractService _recent;

    /// <summary>
    /// Creates a fake contract for SJ.
    /// </summary>
    /// <param name="logger">The logging service to use.</param>
    public FakeContractRepository(ILogger<FakeContractRepository> logger)
    {
        _logger = logger;
        _recent = new RecentContractService(new Collection<Contract>());
    }

    /// <inheritdoc />
    public IEnumerable<Contract> All => new List<Contract>(Contracts);

    /// <inheritdoc />
    public IEnumerable<Contract> Recent => _recent.FetchRecentContracts();

    /// <inheritdoc />
    public IEnumerable<Contract> Favorites => Contracts.Where(contract => contract.IsFavorite);

    private DbSet<Contract> Contracts { get; set; } = null!;

    /// <inheritdoc />
    public void Add(Contract contract)
    {
        _ = Contracts.Add(contract);
        _ = SaveChanges();
        _logger.LogInformation(
            "Added a new contract with name {Name} and id {Id} to the database",
            contract.Name,
            contract.Id);
    }

    /// <inheritdoc />
    public void AddRecent(Contract contract)
    {
        _recent.Add(contract);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        _recent.Remove(id); // This line should not exist when there is an actual database (it will remove any relations)
        Contract? toRemove = Contracts.Find(id);
        if (toRemove is null)
            return false;

        _ = Contracts.Remove(toRemove);

        int changes = 0;
        try
        {
            changes = SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not remove contract from database: {Message}", e.Message);
        }

        // If any changes were made, then the remove operation succeeded.
        return changes > 0;
    }

    /// <inheritdoc />
    public Contract? FetchContract(Guid id)
    {
        return Contracts.Find(id);
    }

    /// <inheritdoc />
    public void UpdateContract(Contract updatedContract)
    {
        _ = Contracts.Update(updatedContract);

        try
        {
            _ = SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not update contract in database: {Message}", e.Message);
        }
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Note: this connection string should be stored in an environment variable away from the source code.
        // If you are replacing this connection string with actual credentials to a real database, don't
        // just replace the string here in the source code, use an environment variable instead.
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
        _logger.LogInformation("Established a new connection to the postgres database");
    }
}
