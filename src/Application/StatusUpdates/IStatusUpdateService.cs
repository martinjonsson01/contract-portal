using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// A way of interacting with and manipulating status updates.
/// </summary>
public interface IStatusUpdateService
{
    /// <summary>
    /// Adds a new status update.
    /// </summary>
    /// <param name="newUpdate">The new status update.</param>
    void Add(StatusUpdate newUpdate);

    /// <summary>
    /// Fetches all of the status updates.
    /// </summary>
    /// <returns>All status updates.</returns>
    IEnumerable<StatusUpdate> FetchAll();
}
