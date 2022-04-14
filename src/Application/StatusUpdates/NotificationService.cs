using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// Creates and manipulates notification status updates, which can be used to issue general information.
/// </summary>
public class NotificationService : IStatusUpdateService
{
    private readonly IStatusUpdateRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="repository">A way of storing and retrieving status updates.</param>
    public NotificationService(IStatusUpdateRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public void Add(StatusUpdate newUpdate)
    {
        _repository.Add(newUpdate);
    }

    /// <inheritdoc />
    public IEnumerable<StatusUpdate> FetchAll()
    {
        return _repository.All;
    }
}
