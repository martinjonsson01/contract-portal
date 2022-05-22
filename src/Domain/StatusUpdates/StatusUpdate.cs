namespace Domain.StatusUpdates;

/// <summary>
/// Contains information about a notable event.
/// </summary>
public class StatusUpdate
{
    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets at which level of urgency the information in this status update is.
    /// </summary>
    public AlertLevel Alert { get; set; } = AlertLevel.Information;

    /// <summary>
    /// Gets or sets when this status update was generated.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the text describing the event.
    /// </summary>
    public string Text { get; set; } = "Ingen text angiven.";
}
