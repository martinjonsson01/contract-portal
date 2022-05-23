using System.Data;

using Application.Configuration;
using Application.StatusUpdates;

using Domain.StatusUpdates;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches status updates from an Entity Framework Core database.
/// </summary>
public class EFStatusUpdatesRepository : IStatusUpdateRepository
{
    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFStatusUpdatesRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFStatusUpdatesRepository(EFDatabaseContext context, ILogger<EFUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public IEnumerable<StatusUpdate> All => new List<StatusUpdate>(StatusUpdates);

    private DbSet<StatusUpdate> StatusUpdates => _context.StatusUpdates;

    /// <inheritdoc />
    public void Add(StatusUpdate statusUpdate)
    {
        _ = StatusUpdates.Add(statusUpdate);
        _ = _context.SaveChanges();
        _logger.LogInformation("Added a new status update with alert-level {Level} and text {Text} to the database", statusUpdate.Alert, statusUpdate.Text);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        StatusUpdate? toRemove = StatusUpdates.Find(id);
        if (toRemove is null)
            return false;

        _ = StatusUpdates.Remove(toRemove);

        int changes = 0;
        try
        {
            changes = _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not remove notification from database: {Message}", e.Message);
        }

        // If any changes were made, then the remove operation succeeded.
        return changes > 0;
    }
}
