using Application.Exceptions;
using Application.Users;
using Domain.Users;

using Microsoft.AspNetCore.Authorization;
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
    [Authorize("AdminOnly")]
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

    /// <summary>
    /// Removes the specified user.
    /// </summary>
    /// <param name="id">Id of the user to be removed.</param>
    /// <returns>If the user was successfully removed.</returns>
    [HttpDelete("{id:guid}")]
    [Authorize("AdminOnly")]
    public IActionResult Remove(Guid id)
    {
        return _users.Remove(id) ?
            Ok() :
            NotFound();
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All the stored users.</returns>
    [HttpGet]
    public IEnumerable<User> GetAll()
    {
        return _users.FetchAllUsers();
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="userInfo">The username and password.</param>
    /// <returns>An authentication token that can be used to identify the user.</returns>
    [HttpPost("authenticate")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Authenticate(User userInfo)
    {
        try
        {
            AuthenticateResponse authResponse = _users.Authenticate(userInfo);
            return Ok(authResponse);
        }
        catch (UserDoesNotExistException e)
        {
            Logger.LogError("Could not authenticate user: {Exception}", e.Message);
            return BadRequest();
        }
    }
}
