using System.Data;

using Application.Contracts;
using Application.Users;

using Domain.Contracts;
using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches recent contracts from an Entity Framework Core database.
/// </summary>
public class EFRecentContractRepository : IRecentContractRepository
{
    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFRecentContractRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFRecentContractRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFRecentContractRepository(EFDatabaseContext context, ILogger<EFRecentContractRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    private DbSet<User> Users => _context.Users;

    /// <inheritdoc />
    public void Add(Guid userId, Contract contract)
    {
        User? user = Users.Include(user => user.RecentlyViewContracts).FirstOrDefault(user => user.Id == userId);
        if (user is null)
            throw new UserDoesNotExistException(userId);

        RecentlyViewedContract? recentlyViewedContract =
            user.RecentlyViewContracts.SingleOrDefault(recentContract => recentContract.ContractId == contract.Id);

        // Checks whether to create a new recent contract or just update the viewed time.
        if (recentlyViewedContract == null)
        {
            var recentContract = new RecentlyViewedContract(contract.Id, user.Id);
            user.RecentlyViewContracts.Add(recentContract);
        }
        else
        {
            recentlyViewedContract.LastViewed = DateTime.Now;
        }

        try
        {
            _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not add contract to recently viewed for user to database: {Message}", e.Message);
        }
    }

    /// <inheritdoc />
    public void Remove(RecentlyViewedContract contractToRemove)
    {
        _context.RecentlyViewedContracts.Remove(contractToRemove);
        _ = _context.SaveChanges();
        _logger.LogInformation(
            "Removed contract with id {ContractId} from user with id {UserId}",
            contractToRemove.ContractId,
            contractToRemove.UserId);
    }

    /// <inheritdoc />
    public IList<RecentlyViewedContract> FetchRecentContracts(Guid userId)
    {
        User? user = Users.Include(user => user.RecentlyViewContracts).FirstOrDefault(user => user.Id == userId);
        if (user is null)
            throw new UserDoesNotExistException(userId);

        return user.RecentlyViewContracts;
    }
}
