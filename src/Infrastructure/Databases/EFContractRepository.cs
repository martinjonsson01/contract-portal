using System.Data;

using Application.Contracts;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public sealed class EFContractRepository : IContractRepository
{
    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFContractRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFContractRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFContractRepository(EFDatabaseContext context, ILogger<EFContractRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> All => Contracts.Include(contract => contract.Tags)
                                                 .Include(contract => contract.AdditionalDocument)
                                                 .OrderBy(contract => contract.Name)
                                                 .ToList();

    private DbSet<Contract> Contracts => _context.Contracts;

    /// <inheritdoc />
    public void Add(Contract contract)
    {
        _ = Contracts.Add(contract);
        _ = _context.SaveChanges();
        _logger.LogInformation(
            "Added a new contract with name {Name} and id {Id} to the database",
            contract.Name,
            contract.Id);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        Contract? toRemove = Contracts.Find(id);
        if (toRemove is null)
            return false;

        _ = Contracts.Remove(toRemove);

        int changes = 0;
        try
        {
            changes = _context.SaveChanges();
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
        return Contracts.Include(contract => contract.Tags)
                        .Include(contract => contract.AdditionalDocument)
                        .FirstOrDefault(contract => contract.Id == id);
    }

    /// <inheritdoc />
    public void UpdateContract(Contract updatedContract)
    {
        Contract? oldContract = FetchContract(updatedContract.Id);
        if (oldContract is not null)
            Contracts.Remove(oldContract);

        _ = Contracts.Add(updatedContract);

        try
        {
            _ = _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not update contract in database: {Message}", e.Message);
        }
    }
}
