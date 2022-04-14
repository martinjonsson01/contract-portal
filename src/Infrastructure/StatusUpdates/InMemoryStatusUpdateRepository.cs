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
        new StatusUpdate(), new StatusUpdate(), new StatusUpdate(), new StatusUpdate(),
    };

    /// <inheritdoc />
    public IEnumerable<StatusUpdate> All => new List<StatusUpdate>(_statusUpdates);
}
