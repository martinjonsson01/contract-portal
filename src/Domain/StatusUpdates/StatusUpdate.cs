namespace Domain.StatusUpdates;

/// <summary>
/// Contains information about a notable event.
/// </summary>
public class StatusUpdate
{
    /// <summary>
    /// Gets or sets at which level of urgency the information in this status update is.
    /// </summary>
    public AlertLevel Alert { get; set; } = AlertLevel.Information;

    /// <summary>
    /// Gets or sets when this status update was generated.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
