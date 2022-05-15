using Application.Contracts;
using Blazorise.Extensions;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for recent contracts.
/// </summary>
[ApiController]
[Route("api/v1/users/{username}/[controller]")]
public class RecentsController : BaseApiController<RecentsController>
{
    private readonly IRecentContractService _recent;

    /// <summary>
    /// Constructs recent contract API.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="recent">The recent contract logic.</param>
    public RecentsController(ILogger<RecentsController> logger, IRecentContractService recent)
        : base(logger)
    {
        _recent = recent;
    }

    /// <summary>
    /// Gets all recently viewed contracts.
    /// </summary>
    /// <param name="username">The logged in user.</param>
    /// <returns>All recently viewed contracts.</returns>
    [HttpGet]
    public IEnumerable<Contract> RecentContracts(string username)
    {
        return username.IsNullOrEmpty() ? new List<Contract>() : _recent.FetchRecentContracts(username);
    }

    /// <summary>
    /// Adds a contract as recently viewed.
    /// </summary>
    /// <param name="username">The logged in user.</param>
    /// <param name="contract">The contract to add.</param>
    /// <returns>Returns success after it has added the contract to recently viewed.</returns>
    [HttpPost]
    public IActionResult Add(string username, Contract contract)
    {
        _recent.Add(username, contract);
        return Ok();
    }
}
