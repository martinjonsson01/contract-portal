using Application.Contracts;
using Application.Exceptions;
using Application.Users;
using Blazorise.Extensions;
using Domain.Contracts;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    [HttpPut]
    [Authorize("AdminOnly")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] User user)
    {
        try
        {
            _users.Add(user);
        }
        catch (IdentifierAlreadyTakenException)
        {
            _users.UpdateUser(user);
        }
        catch (UserNameTakenException)
        {
            return BadRequest($"Name {user.Name} is already taken");
        }

        return Ok();
    }

    /// <summary>
    /// Updates the user.
    /// </summary>
    /// <param name="patchDocument">The patch to use to update the user.</param>
    /// <param name="id">The id of the user to update.</param>
    /// <returns>The updated user.</returns>
    [HttpPatch("{id:guid}")]
    [Authorize("AdminOnly")]
    public IActionResult UpdateUser([FromBody] JsonPatchDocument<User> patchDocument, Guid id)
    {
        User user = _users.FetchUser(id);

        // Only need to encrypt the password if it's been modified, otherwise
        // we'd just be re-encrypting an already encrypted password.
        bool shouldEncryptPassword = patchDocument.Operations.Any(operation => operation.path == "/Password");

        patchDocument.ApplyTo(user, ModelState);
        _users.UpdateUser(user, shouldEncryptPassword);

        // Can't place model in an invalid state at the moment, as all states are considered valid.
        // In the future we might want to add model validation here.
        return new ObjectResult(user);
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
        return _users.Remove(id) ? Ok() : NotFound();
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All the stored users.</returns>
    [HttpGet]
    [Authorize("AdminOnly")]
    public IEnumerable<User> GetAll()
    {
        return _users.FetchAllUsers();
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="user">The user to authenticate.</param>
    /// <returns>An authentication token that can be used to identify the user.</returns>
    [HttpPost("authenticate")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Authenticate(User user)
    {
        try
        {
            AuthenticateResponse authResponse = _users.Authenticate(user.Name, user.Password);
            return Ok(authResponse);
        }
        catch (UserDoesNotExistException e)
        {
            Logger.LogError("Can't authenticate a user that does not exist: {Exception}", e.Message);
            return Unauthorized();
        }
        catch (InvalidPasswordException e)
        {
            Logger.LogError("An incorrect password was given for the user: {Exception}", e.Message);
            return Unauthorized();
        }
    }
}
