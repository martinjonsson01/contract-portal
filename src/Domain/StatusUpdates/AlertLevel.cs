namespace Domain.StatusUpdates;

/// <summary>
/// How urgent an alert is.
/// </summary>
public enum AlertLevel
{
    /// <summary>
    /// To alert about new information.
    /// </summary>
    Information,

    /// <summary>
    /// To warn about something.
    /// </summary>
    Warning,

    /// <summary>
    /// Something urgent needs to be attended to.
    /// </summary>
    Urgent,

    /// <summary>
    /// Something critical has occured.
    /// </summary>
    Critical,
}
