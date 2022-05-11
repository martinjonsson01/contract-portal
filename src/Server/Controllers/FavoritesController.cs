using Application.Contracts;
using Application.MessagePassing;
using Application.Users;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for favorites.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class FavoritesController : BaseApiController<FavoritesController>
{
    private readonly IUserService _users;

    /// <summary>
    /// Constructs favorite API.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="users">The users logic.</param>
    public FavoritesController(ILogger<FavoritesController> logger, IUserService users)
        : base(logger)
    {
        _users = users;
    }

    /// <summary>
    /// Adds a favorite or removes it if it already exists.
    /// </summary>
    /// <param name="setFavoriteContract">The favorite to add or remove.</param>
    /// <returns>Whether the change succeeded.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Change(SetFavoriteContract setFavoriteContract)
    {
        try
        {
            if (setFavoriteContract.IsFavorite)
            {
                _users.AddFavorite(setFavoriteContract.UserName, setFavoriteContract.ContractId);
            }
            else
            {
                _ = _users.RemoveFavorite(setFavoriteContract.UserName, setFavoriteContract.ContractId);
            }

            return Ok();
        }
        catch (UserDoesNotExistException)
        {
            return BadRequest();
        }
        catch (ContractDoesNotExistException)
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Checks if a certain contract is marked as favorite by a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>Whether the contract is marked as favorite by the user.</returns>
    [HttpGet("{userName}/{contractId:guid}")]
    public IActionResult GetIsFavorite(string userName, Guid contractId)
    {
        return _users.IsFavorite(userName, contractId) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Gets all contracts marked as favorite by a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <returns>All contracts marked as favorite by the user.</returns>
    [HttpGet("{userName}")]
    public IEnumerable<Contract> GetAll(string userName)
    {
        return _users.FetchAllFavorites(userName);
    }
}
