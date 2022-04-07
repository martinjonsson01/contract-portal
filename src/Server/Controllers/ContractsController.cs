using Application.Contracts;

using Domain.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// WebAPI for contracts.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ContractsController : Controller
{
    private readonly IContractService _contract;

    /// <summary>
    /// Constructs contract API.
    /// </summary>
    /// <param name="contract">The contract logic.</param>
    public ContractsController(IContractService contract)
    {
        _contract = contract;
    }

    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <returns>All contracts.</returns>
    [HttpGet("All")]
    public IEnumerable<Contract> AllContracts()
    {
        return _contract.FetchAllContracts();
    }
}
