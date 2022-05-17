using Domain.Contracts;
using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Databases;

/// <summary>
/// A way to access the entities stored in the database.
/// </summary>
internal interface IDatabaseContext
{
    /// <summary>
    /// Gets or sets the <see cref="Contract"/>s in the database.
    /// </summary>
    DbSet<Contract> Contracts { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="User"/>s in the database.
    /// </summary>
    DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RecentlyViewedContract"/>s in the database.
    /// </summary>
    DbSet<RecentlyViewedContract> RecentlyViewedContracts { get; set; }

    /// <summary>
    /// Test.
    /// </summary>
    /// <returns>Testing.</returns>
    int SaveChanges();

    /// <summary>
    /// Test.
    /// </summary>
    /// <param name="entity">Testar.</param>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>Testes.</returns>
    EntityEntry Entry<TEntity>(TEntity entity)
        where TEntity : class;
}
