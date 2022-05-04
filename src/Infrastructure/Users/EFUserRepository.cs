using System.Data;
using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Users;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public class EFUserRepository : DbContext, IUserRepository, IRecentContractRepository
{
    private readonly ILogger<EFUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFUserRepository"/> class.
    /// </summary>
    /// <param name="options">The database configuration options.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFUserRepository(
        DbContextOptions<EFUserRepository> options,
        ILogger<EFUserRepository> logger)
        : base(options)
    {
        _logger = logger;
        _logger.LogInformation("Established a new connection to the database");
    }

    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(Users);

    private DbSet<User> Users { get; set; } = null!;

    /// <inheritdoc />
    public void Add(User user)
    {
        _ = Users.Add(user);
        _ = SaveChanges();
        _logger.LogInformation("Added a new user with name {Name} and id {Id} to the database", user.Name, user.Id);
    }

    /// <inheritdoc />
    public void Add(string id, Contract contract)
    {
        User user = GetUserByUserName(id);
        _ = user.RecentlyViewContracts.AddFirst(contract);
        try
        {
            _ = SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not add contract to recently viewed for user to database: {Message}", e.Message);
        }
    }

    /// <inheritdoc/>
    void IRecentContractRepository.Remove(Guid id)
    {
        foreach (User user in Users)
        {
            Contract contractToRemove = user.RecentlyViewContracts.First(contract => contract.Id == id);
            _ = user.RecentlyViewContracts.Remove(contractToRemove);
        }

        try
        {
            _ = SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not remove contract from recently viewed for all user in database: {Message}", e.Message);
        }
    }

    /// <inheritdoc />
    public void RemoveLast(string id)
    {
        User user = GetUserByUserName(id);
        user.RecentlyViewContracts.RemoveLast();
        _ = SaveChanges();
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
    public bool Exists(string username)
    {
        return Users.Any(user => user.Name == username);
    }

    /// <inheritdoc />
    public LinkedList<Contract> FetchRecentContracts(string id)
    {
        User user = GetUserByUserName(id);
        Console.WriteLine($"Fetching From user{user.Name}");
        Console.WriteLine($"{user.RecentlyViewContracts.Count}");
        foreach (Contract recentContract in user.RecentlyViewContracts)
        {
            Console.WriteLine(recentContract.Name);
        }

        return user.RecentlyViewContracts ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
            .HasKey(user => user.Id);
    }

    private User GetUserByUserName(string username)
    {
        return Users.First(user => user.Name == username);
    }
}
