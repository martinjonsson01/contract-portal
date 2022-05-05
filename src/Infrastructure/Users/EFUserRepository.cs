using System.Data;

using Application.Users;

using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Users;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public sealed class EFUserRepository : DbContext, IUserRepository
{
    private const string AdminUserName = "admin";

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

        // Creates the database if it is not already created.
        _ = Database.EnsureCreated();
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
    public User? Fetch(string username)
    {
        return Users.First(user => user.Name == username);
    }

    /// <inheritdoc />
    public bool Exists(string username)
    {
        return Users.Any(user => user.Name == username);
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

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
                        .HasKey(user => user.Id);
    }

    private void CreateAdmin()
    {
        var admin = new User { Name = AdminUserName, Company = "Prodigo", LatestPaymentDate = DateTime.MaxValue, };
        _ = Users.Add(admin);
        try
        {
            _ = SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not create admin: {Message}", e.Message);
        }
    }
}
