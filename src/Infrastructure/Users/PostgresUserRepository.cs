using Application.Users;

using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Users;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public class PostgresUserRepository : DbContext, IUserRepository
{
    private readonly ILogger<PostgresUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresUserRepository"/> class.
    /// </summary>
    /// <param name="options">The database configuration options.</param>
    /// <param name="logger">The logging service to use.</param>
    public PostgresUserRepository(
        DbContextOptions<PostgresUserRepository> options,
        ILogger<PostgresUserRepository> logger)
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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
                        .HasKey(user => user.Id);
    }
}
