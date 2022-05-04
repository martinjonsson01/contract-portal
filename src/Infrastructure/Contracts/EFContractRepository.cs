using System.Data;

using Application.Contracts;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Contracts;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public class EFContractRepository : DbContext, IContractRepository
{
    private readonly ILogger<EFContractRepository> _logger;
    private readonly IRecentContractService _recent;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFContractRepository"/> class.
    /// </summary>
    /// <param name="options">The database configuration options.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFContractRepository(
        DbContextOptions<EFContractRepository> options,
        ILogger<EFContractRepository> logger)
        : base(options)
    {
        _logger = logger;
        _recent = new RecentContractService();
        _logger.LogInformation("Established a new connection to the database");
    }

    /// <inheritdoc />
    public IEnumerable<Contract> All => Contracts.Include(contract => contract.Tags).ToList();

    /// <inheritdoc />
    public IEnumerable<Contract> Recent => _recent.FetchRecentContracts(" ");

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
        _recent.Add(" ", contract);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        _recent.Remove(id); // This line should not exist when recents are included in the database
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
        Contract? oldContract = FetchContract(updatedContract.Id);
        if (oldContract is null)
            _ = Contracts.Add(updatedContract);
        else
            Entry(oldContract).CurrentValues.SetValues(updatedContract);

        try
        {
            _ = SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not update contract in database: {Message}", e.Message);
        }
    }
}
