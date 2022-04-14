using Application.StatusUpdates;

using Domain.StatusUpdates;

namespace Infrastructure.StatusUpdates;

/// <summary>
/// Stores status updates in-memory.
/// </summary>
public class InMemoryStatusUpdateRepository : IStatusUpdateRepository
{
    private readonly ICollection<StatusUpdate> _statusUpdates = new List<StatusUpdate>
    {
        new StatusUpdate { Alert = AlertLevel.Urgent, Timestamp = DateTime.Now.Subtract(TimeSpan.FromDays(12)), },
        new StatusUpdate { Alert = AlertLevel.Information, Timestamp = DateTime.Now.Subtract(TimeSpan.FromMinutes(16)), },
        new StatusUpdate { Alert = AlertLevel.Critical, Timestamp = DateTime.Now.Subtract(TimeSpan.FromDays(30 * 3)), },
        new StatusUpdate { Alert = AlertLevel.Warning, Timestamp = DateTime.Now.Subtract(TimeSpan.FromHours(3)), },
    };

    /// <inheritdoc />
    public IEnumerable<StatusUpdate> All => new List<StatusUpdate>(_statusUpdates);
}
