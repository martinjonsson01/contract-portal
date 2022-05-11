using System.Data;
using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public sealed class EFUserRepository : IUserRepository, IRecentContractRepository
{
    private const string AdminUserName = "admin";

    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFUserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFUserRepository(EFDatabaseContext context, ILogger<EFUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(Users);

    private DbSet<User> Users => _context.Users;

    /// <inheritdoc />
    public void Add(User user)
    {
        _ = Users.Add(user);
        _ = _context.SaveChanges();
        _logger.LogInformation("Added a new user with name {Name} and id {Id} to the database", user.Name, user.Id);
    }

    /// <inheritdoc />
    public void AddRecent(string username, Contract contract)
    {
        User user = GetUserByUserName(username);

        RecentlyViewedContract? recentlyViewedContract =
            user.RecentlyViewContracts.SingleOrDefault(recentContract => recentContract.ContractId == contract.Id);

        // Check whether to create a new recent contract or just update the viewed time.
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
    public void RemoveRecent(RecentlyViewedContract contractToRemove)
    {
        _context.RecentlyViewedContracts.Remove(contractToRemove);
        _ = _context.SaveChanges();
        _logger.LogInformation(
            "Removed contract with id {ContractId} from user with id {UserId}",
            contractToRemove.ContractId,
            contractToRemove.UserId);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        User? toRemove = Users.Find(id);
        if (toRemove is null)
            return false;

        _ = Users.Remove(toRemove);

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
    public bool Exists(string username)
    {
        return Users.Any(user => user.Name == username);
    }

    /// <inheritdoc />
    public IList<RecentlyViewedContract> FetchRecentContracts(string id)
    {
        User user = GetUserByUserName(id);
        return user.RecentlyViewContracts;
    }

    /// <inheritdoc />
    public User? Fetch(string username)
    {
        return Users.FirstOrDefault(user => user.Name == username);
    }

    /// <inheritdoc />
    public void EnsureAdminCreated()
    {
        if (!Users.Any(user => user.Name == AdminUserName))
            CreateAdmin();
    }

    private void CreateAdmin()
    {
        var admin = new User { Name = AdminUserName, Company = "Prodigo", LatestPaymentDate = DateTime.MaxValue };
        _ = Users.Add(admin);
        try
        {
            _ = _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not create admin: {Message}", e.Message);
        }
    }

    private User GetUserByUserName(string username)
    {
        User user = Users.Include(user => user.RecentlyViewContracts).First(user => user.Name == username);
        return user;
    }
}
