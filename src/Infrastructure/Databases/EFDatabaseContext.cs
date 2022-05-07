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

    /// <summary>
    /// Gets or sets the <see cref="Contract"/>s in the database.
    /// </summary>
    public DbSet<Contract> Contracts { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="User"/>s in the database.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <inheritdoc/>
    public new EntityEntry Entry<TEntity>(TEntity entity)
        where TEntity : class
    {
        return base.Entry(entity);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder
        .Entity<User>()
        .HasMany(u => u.Contracts)
        .WithMany(c => c.Users)
        .UsingEntity(j => j.ToTable("UserContracts"));
    }
}
