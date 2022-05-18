using Application.StatusUpdates;

using Domain.StatusUpdates;

using Microsoft.EntityFrameworkCore;
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
        _logger.LogInformation("Added a new status update");
    }
}
