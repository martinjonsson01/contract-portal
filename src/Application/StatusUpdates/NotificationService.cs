using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// Creates and manipulates notification status updates, which can be used to issue general information.
/// </summary>
public class NotificationService : IStatusUpdateService
{
    private readonly IStatusUpdateRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="repo">A way of storing and retrieving status updates.</param>
    public NotificationService(IStatusUpdateRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public IEnumerable<StatusUpdate> FetchAll()
    {
        return _repo.All;
    }

    /// <inheritdoc />
    public void Add(StatusUpdate statusUpdate)
    {
        _repo.Add(statusUpdate);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _repo.Remove(id);
    }
}
