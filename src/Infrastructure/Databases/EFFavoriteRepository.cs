using Application.Contracts;
using Application.FavoriteContracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches favorites from an Entity Framework Core database.
/// </summary>
public sealed class EFFavoriteRepository : IFavoriteContractRepository
{
    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFFavoriteRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFFavoriteRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFFavoriteRepository(EFDatabaseContext context, ILogger<EFFavoriteRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public void Add(string userName, Guid contractId)
    {
        User user = FetchUser(userName);
        Contract contract = FetchContract(contractId);

        user.Favorites.Add(contract);
        _ = _context.SaveChanges();
        _logger.LogInformation("Contract {ContractId} was added as favorite for user {UserName}", contractId, userName);
    }

    /// <inheritdoc />
    public bool Remove(string userName, Guid contractId)
    {
        User user = FetchUser(userName);
        Contract contract = FetchContract(contractId);
        bool result = user.Favorites.Remove(contract);
        _context.SaveChanges();

        _logger.LogInformation("Contract {ContractId} was removed as favorite from user {UserName}", contractId, userName);
        return result;
    }

    private User FetchUser(string userName)
    {
        try
        {
            return _context.Users.Where(s => s.Name == userName).Include(s => s.Favorites).First();
        }
        catch (InvalidOperationException)
        {
            throw new UserDoesNotExistException();
        }
    }

    private Contract FetchContract(Guid contractId)
    {
        try
        {
            return _context.Contracts.Where(s => s.Id == contractId).First();
        }
        catch (InvalidOperationException)
        {
            throw new ContractDoesNotExistException();
        }
    }
}
