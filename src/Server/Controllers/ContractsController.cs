using Application.Contracts;
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
    /// <param name="file">The image to upload and store on the server.</param>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The uploaded file is not a valid image.</response>
    [HttpPost("new/image")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadImageAsync(IFormFile file)
    {
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
}
