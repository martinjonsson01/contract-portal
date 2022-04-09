﻿using Application.Contracts;
using Application.Exceptions;
using Application.Images;

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
    private readonly IContractService _contracts;
    private readonly IImageRepository _images;
    private readonly ILogger<ContractsController> _logger;

    /// <summary>
    /// Constructs contract API.
    /// </summary>
    /// <param name="contracts">The contract logic.</param>
    /// <param name="images">The place to store images.</param>
    /// <param name="logger">The logging provider.</param>
    public ContractsController(IContractService contracts, IImageRepository images, ILogger<ContractsController> logger)
    {
        _contracts = contracts;
        _images = images;
        _logger = logger;
    }

    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <returns>All contracts.</returns>
    [HttpGet("All")]
    public IEnumerable<Contract> AllContracts()
    {
        return _contracts.FetchAllContracts();
    }

    /// <summary>
    /// Uploads a new contract image file.
    /// </summary>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The uploaded file is not a valid image.</response>
    [HttpPost("new/image")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadImageAsync()
    {
        IFormFile file = Request.Form.Files[0];
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        _logger.LogInformation("Trying to upload an image file: {Name}", file.Name);
        try
        {
            return await _images.StoreAsync(file.OpenReadStream()).ConfigureAwait(false);
        }
        catch (InvalidImageException e)
        {
            _logger.LogInformation("Error occured during image upload: {Error}", e.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// Creates a new contract.
    /// </summary>
    /// <param name="contract">The contract to add.</param>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The ID of the contract was already taken.</response>
    [HttpPost("new/contract")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateContract(Contract contract)
    {
        try
        {
            _contracts.Add(contract);
        }
        catch (IdentifierAlreadyTakenException e)
        {
            _logger.LogInformation("ID of contract was already taken: {Error}", e.Message);
            return BadRequest();
        }

        return Ok();
    }
}
