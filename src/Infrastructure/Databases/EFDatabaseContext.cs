using Domain.Contracts;
using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Represents the Entity Framework Core database, containing all of the different tables.
/// </summary>
public sealed class EFDatabaseContext : DbContext, IDatabaseContext
{
    private readonly ILogger<EFDatabaseContext> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFDatabaseContext"/> class.
    /// </summary>
    /// <param name="options">The database configuration options.</param>
    /// <param name="logger">The logging service to use.</param>
    /// <param name="config">The configuration of the current environment.</param>
    public EFDatabaseContext(
        DbContextOptions<EFDatabaseContext> options,
        ILogger<EFDatabaseContext> logger,
        IConfiguration config)
        : base(options)
    {
        _logger = logger;
        _logger.LogInformation("Established a new connection to the database");
    }

    /// <inheritdoc />
    public DbSet<Contract> Contracts { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<User> Users { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<RecentlyViewedContract> RecentlyViewedContracts { get; set; } = null!;

    /// <inheritdoc/>
    public new EntityEntry Entry<TEntity>(TEntity entity)
        where TEntity : class
    {
        return base.Entry(entity);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
                        .HasMany(p => p.Favorites)
                        .WithMany("FavoritedBy")
                        .UsingEntity(join => join.ToTable("UserFavoriteContracts"));
        _ = modelBuilder.Entity<User>()
                        .HasKey(user => user.Id);
        _ = modelBuilder.Entity<User>()
                        .Property(user => user.Name)
                        .UseCollation("Finnish_Swedish_CS_AS");

        _ = modelBuilder.Entity<Contract>()
                        .HasKey(contract => contract.Id);
        _ = modelBuilder.Entity<Contract>()
                        .HasMany<RecentlyViewedContract>()
                        .WithOne()
                        .HasForeignKey(nameof(RecentlyViewedContract.ContractId));

        _ = modelBuilder.Entity<RecentlyViewedContract>()
                        .HasKey(recentContract => new { recentContract.ContractId, recentContract.UserId, });
    }
}
