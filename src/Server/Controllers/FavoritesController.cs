using Application.FavoriteContracts;
using Application.MessagePassing;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for users.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class FavoritesController : BaseApiController<FavoritesController>
{
    private readonly IFavoriteContractService _favorites;

    /// <summary>
    /// Constructs user API.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="favorites">The user logic.</param>
    public FavoritesController(ILogger<FavoritesController> logger, IFavoriteContractService favorites)
        : base(logger)
    {
        _favorites = favorites;
    }

    /// <summary>
    /// Creates a new contract.
    /// </summary>
    /// <param name="setFavorite">The contract to add.</param>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The ID of the contract was already taken.</response>
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
    /// Gets all users.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <param name="contractId">Id of contract.</param>
    /// <returns>All the stored users.</returns>
    [HttpGet("{userName}/{contractId:guid}")]
    public IActionResult GetIsFavorite(string userName, Guid contractId)
    {
        return _favorites.CheckIfFavorite(userName, contractId) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns>All the stored users.</returns>
    [HttpGet("{userName}")]
    public IEnumerable<Contract> GetIsFavorite(string userName)
    {
        return _favorites.FetchAllFavorites(userName);
    }
}
