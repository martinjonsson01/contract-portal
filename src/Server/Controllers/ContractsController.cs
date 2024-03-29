﻿using Application.Contracts;
using Application.Exceptions;
using Blazorise.Extensions;
using Domain.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    public ContractsController(
        ILogger<ContractsController> logger,
        IContractService contracts)
        : base(logger)
    {
        _contracts = contracts;
    }

    /// <summary>
    /// Updates the contract.
    /// </summary>
    /// <param name="patchDocument">The patch to use to update the contract.</param>
    /// <param name="id">The id of the contract to update.</param>
    /// <returns>The updated contract.</returns>
    [HttpPatch("{id:guid}")]
    [Authorize("AdminOnly")]
    public IActionResult UpdateContract([FromBody] JsonPatchDocument<Contract> patchDocument, Guid id)
    {
        Contract contract = _contracts.FetchContract(id);
        patchDocument.ApplyTo(contract, ModelState);
        _contracts.UpdateContract(contract);

        // Can't place model in an invalid state at the moment, as all states are considered valid.
        // In the future we might want to add model validation here.
        return new ObjectResult(contract);
    }

    /// <summary>
    /// Creates a new contract.
    /// </summary>
    /// <param name="contract">The contract to put.</param>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The ID of the contract was already taken.</response>
    [HttpPut]
    [Authorize("AdminOnly")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateContract([FromBody] Contract contract)
    {
        try
        {
            _contracts.Add(contract);
        }
        catch (IdentifierAlreadyTakenException)
        {
            _contracts.UpdateContract(contract);
        }

        return Ok();
    }

    /// <summary>
    /// Removes the specified contract.
    /// </summary>
    /// <param name="id">Id of the contract to be removed.</param>
    /// <returns>If the contract was successfully removed.</returns>
    [HttpDelete("{id:guid}")]
    [Authorize("AdminOnly")]
    public IActionResult Remove(Guid id)
    {
        return _contracts.Remove(id) ? Ok() : NotFound();
    }

    /// <summary>
    /// Searches for contracts that match the given query and returns the resulting contracts.
    /// </summary>
    /// <param name="query">The query to filter contracts by.</param>
    /// <returns>The contracts that match the search query.</returns>
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<Contract>> Search(string? query)
    {
        query ??= string.Empty;

        if (User?.Identity?.IsAuthenticated ?? false)
            return Ok(_contracts.Search(query));

        return Ok(_contracts.SearchUnauthorized(query));
    }
}
