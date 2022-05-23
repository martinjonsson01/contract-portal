using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// Represents a way of storing and retrieving <see cref="StatusUpdate"/>s.
/// </summary>
public interface IStatusUpdateRepository
{
    /// <summary>
    /// Gets all status updates.
    /// </summary>
    IEnumerable<StatusUpdate> All { get; }

    /// <summary>
    /// Adds a new status update to store.
    /// </summary>
    /// <param name="statusUpdate">The new status update instance.</param>
    void Add(StatusUpdate statusUpdate);

    /// <summary>
    /// Removes a status with the given ID.
    /// </summary>
    /// <param name="id">The id of the status to be removed.</param>
    /// <returns>If the removal was successful.</returns>
    bool Remove(Guid id);
}
