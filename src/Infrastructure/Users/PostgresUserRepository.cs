using Application.Users;

using Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

/// <summary>
/// Mocks fake users.
/// </summary>
public class PostgresUserRepository : DbContext, IUserRepository
{
    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(Users);

    private DbSet<User> Users { get; set; } = null!;

    /// <inheritdoc />
    public void Add(User user)
    {
        _ = Users.Add(user);
        _ = SaveChanges();
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
                        .HasKey(user => user.Id);
    }
}
