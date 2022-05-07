using Application.FavoriteContracts;
using Domain.Contracts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public sealed class EFFavoriteRepository : IFavoriteContractRepository
{
    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFUserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFFavoriteRepository(EFDatabaseContext context, ILogger<EFUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool CheckIfFavorite(string userName, Guid contractId)
    {
        User user = _context.Users.Where(s => s.Name == userName).Include(s => s.Contracts).First();
        Contract contract = _context.Contracts.Where(s => s.Id == contractId).First();
        return user.Contracts.Contains(contract);
    }

    /// <inheritdoc />
    public void Add(string userName, Guid contractId)
    {
        User user = _context.Users.Where(s => s.Name == userName).Include(s => s.Contracts).First();
        Contract contract = _context.Contracts.Where(s => s.Id == contractId).First();

        user.Contracts.Add(contract);
        _ = _context.SaveChanges();
        _logger.LogInformation("A contract was marked as favorite by a user");
    }

    /// <inheritdoc />
    public bool Remove(string userName, Guid contractId)
    {
        User user = _context.Users.Where(s => s.Name == userName).Include(s => s.Contracts).First();
        Contract contract = _context.Contracts.Where(s => s.Id == contractId).First();

        _ = user.Contracts.Remove(contract);
        int changes = _context.SaveChanges();

        _logger.LogInformation("A contract was unfavorited by a user");

        // If any changes were made, then the remove operation succeeded.
        return changes > 0;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchAllFavorites(string userName)
    {
        User user = _context.Users.Where(s => s.Name == userName).Include(s => s.Contracts).First();
        List<Contract> contractsWithoutSelfReference = new();

        foreach (Contract? contract in user.Contracts)
        {
            contract.Users = null!;
            contractsWithoutSelfReference.Add(contract);
        }

        return contractsWithoutSelfReference;
    }
}
