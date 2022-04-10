using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Implementation of common parts of all API controllers.
/// </summary>
/// <typeparam name="TLogging">The type to get the logging service of.</typeparam>
[ApiController]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class BaseApiController<TLogging> : ControllerBase
{
    /// <summary>
    /// Constructs the controller with a given logger.
    /// </summary>
    /// <param name="logger">The logging service to use.</param>
    public BaseApiController(ILogger<TLogging> logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Gets the logging service.
    /// </summary>
    protected ILogger<TLogging> Logger { get; }
}
