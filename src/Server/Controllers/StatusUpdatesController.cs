using Application.StatusUpdates;

using Domain.StatusUpdates;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Endpoints for modifying status update notifications.
/// </summary>
[ApiController]
[Route("api/v1/status-updates")]
public class StatusUpdatesController : BaseApiController<StatusUpdatesController>
{
    private readonly IStatusUpdateService _statusUpdates;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusUpdatesController"/> class.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="statusUpdates">A way of interacting with status updates.</param>
    public StatusUpdatesController(ILogger<StatusUpdatesController> logger, IStatusUpdateService statusUpdates)
        : base(logger)
    {
        _statusUpdates = statusUpdates;
    }

    /// <summary>
    /// Gets all status updates.
    /// </summary>
    /// <response code="200">All status updates were successfully fetched.</response>
    /// <returns>The status updates.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<StatusUpdate> All()
    {
        return _statusUpdates.FetchAll();
    }
}
