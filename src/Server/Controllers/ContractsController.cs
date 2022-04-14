using Application.Contracts;
using Application.Exceptions;

using Domain.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for contracts.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ContractsController : BaseApiController<ContractsController>
{
    private readonly IContractService _contracts;

    /// <summary>
    /// Constructs contract API.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="contracts">The contract logic.</param>
    public ContractsController(ILogger<ContractsController> logger, IContractService contracts)
        : base(logger)
    {
        _contracts = contracts;
    }

    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <returns>All contracts.</returns>
    [HttpGet]
    public IEnumerable<Contract> All()
    {
        return _contracts.FetchAllContracts();
    }

    /// <summary>
    /// Gets all recently viewed contracts.
    /// </summary>
    /// <returns>All recently viewed contracts.</returns>
    [HttpGet("Recent")]
    public IEnumerable<Contract> RecentContracts()
    {
        return _contracts.FetchRecentContracts();
    }

    /// <summary>
    /// Adds a contract as recently viewed.
    /// </summary>
    /// <param name="contract">The contract to add.</param>
    /// <returns>Returns success after it has added the contract to recently viewed.</returns>
    [HttpPost("AddRecent")]
    public IActionResult AddRecent(Contract contract)
    {
        _contracts.AddRecent(contract);
        return Ok();
    }

    /// <summary>
    /// Creates a new contract.
    /// </summary>
    /// <param name="contract">The contract to add.</param>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The ID of the contract was already taken.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateContract(Contract contract)
    {
        try
        {
            _contracts.Add(contract);
        }
        catch (IdentifierAlreadyTakenException e)
        {
            Logger.LogInformation("ID of contract was already taken: {Error}", e.Message);
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Removes the specified contract.
    /// </summary>
    /// <param name="id">Id of the contract to be removed.</param>
    /// <returns>If the contract was successfully removed.</returns>
    [HttpDelete("{id:Guid}")]
    public IActionResult Remove(Guid id)
    {
        return _contracts.Remove(id) ?
            Ok() :
            NotFound();
    }
}
