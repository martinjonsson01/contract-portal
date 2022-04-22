using Application.Exceptions;
using Application.Users;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for users.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : BaseApiController<UsersController>
{
    private readonly IUserService _users;

    /// <summary>
    /// Constructs user API.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="users">The user logic.</param>
    public UsersController(ILogger<UsersController> logger, IUserService users)
        : base(logger)
    {
        _users = users;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>If the user was successfully added.</returns>
    /// <response code="400">The ID of the user was already taken.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(User user)
    {
        try
        {
            _users.Add(user);
        }
        catch (IdentifierAlreadyTakenException e)
        {
            Logger.LogInformation("ID of user was already taken: {Error}", e.Message);
            return BadRequest();
        }

        return Ok();
    }
}
