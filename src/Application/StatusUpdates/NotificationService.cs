using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// Creates and manipulates notification status updates, which can be used to issue general information.
/// </summary>
public class NotificationService : IStatusUpdateService
{
    /// <inheritdoc />
    public void Add(StatusUpdate newUpdate)
    {
    }
}
