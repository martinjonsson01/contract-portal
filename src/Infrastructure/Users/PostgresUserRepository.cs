using System.Data;
using Application.Users;

using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Users;

/// <summary>
/// Stores and fetches users from a PostgreSQL database.
/// </summary>
public class PostgresUserRepository : DbContext, IUserRepository
{
    private readonly ILogger<PostgresUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresUserRepository"/> class.
    /// </summary>
    /// <param name="logger">The logging service to use.</param>
    public PostgresUserRepository(ILogger<PostgresUserRepository> logger)
    {
        _logger = logger;
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
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Note: this connection string should be stored in an environment variable away from the source code.
        // If you are replacing this connection string with actual credentials to a real database, don't
        // just replace the string here in the source code, use an environment variable instead.
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
        _logger.LogInformation("Established a new connection to the postgres database");
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
                        .HasKey(user => user.Id);
    }
}
