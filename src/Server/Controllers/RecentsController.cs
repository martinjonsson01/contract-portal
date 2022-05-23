using Application.Contracts;
using Application.Users;
using Blazorise.Extensions;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for recent contracts.
/// </summary>
[ApiController]
[Route("api/v1/users/{userId:guid}/[controller]")]
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
    /// <param name="userId">The ID of the logged in user.</param>
    /// <returns>All recently viewed contracts.</returns>
    [HttpGet]
    public IEnumerable<Contract> RecentContracts(Guid? userId)
    {
        return userId is null ? new List<Contract>() : _recent.FetchRecentContracts(userId.Value);
    }

    /// <summary>
    /// Adds a contract as recently viewed.
    /// </summary>
    /// <param name="userId">The ID of the logged in user.</param>
    /// <param name="contract">The contract to add.</param>
    [HttpPost]
    public void Add(Guid userId, Contract contract)
    {
        try
        {
            _recent.Add(userId, contract);
        }
        catch (UserDoesNotExistException)
        {
            BadRequest();
        }
    }
}
