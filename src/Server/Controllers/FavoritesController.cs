using Application.FavoriteContracts;
using Application.MessagePassing;
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
    /// Adds a favorite.
    /// </summary>
    /// <param name="setFavorite">The favorite to add.</param>
    /// <returns>Whether the favorite was successfully added.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateContract(SetFavoriteContract setFavorite)
    {
        if (setFavorite is null)
            return BadRequest();

        if (setFavorite.IsFavorite)
        {
            _favorites.Add(setFavorite.UserName, setFavorite.ContractId);
        }
        else
        {
            _ = _favorites.Remove(setFavorite.UserName, setFavorite.ContractId);
        }

        return Ok();
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All the stored users.</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
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
        return _favorites.CheckIfFavorite(userName, contractId) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Gets all contracts marked as favorite by a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <returns>All contracts marked as favorite by the user.</returns>
    [HttpGet("{userName}")]
    public IEnumerable<Contract> GetIsFavorite(string userName)
    {
        return _favorites.FetchAllFavorites(userName);
    }
}
