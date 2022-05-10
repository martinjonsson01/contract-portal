using Application.Contracts;
using Application.FavoriteContracts;
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
    private readonly IFavoriteContractService _favorites;

    /// <summary>
    /// Constructs favorite API.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="favorites">The favorite logic.</param>
    public FavoritesController(ILogger<FavoritesController> logger, IFavoriteContractService favorites)
        : base(logger)
    {
        _favorites = favorites;
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
                _favorites.Add(setFavoriteContract.UserName, setFavoriteContract.ContractId);
            }
            else
            {
                _ = _favorites.Remove(setFavoriteContract.UserName, setFavoriteContract.ContractId);
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
        return _favorites.IsFavorite(userName, contractId) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Gets all contracts marked as favorite by a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <returns>All contracts marked as favorite by the user.</returns>
    [HttpGet("{userName}")]
    public IEnumerable<Contract> GetAll(string userName)
    {
        return _favorites.FetchAll(userName);
    }
}
