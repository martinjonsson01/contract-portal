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
    private readonly IImageService _images;

    /// <summary>
    /// Constructs contract API.
    /// </summary>
    /// <param name="contracts">The contract logic.</param>
    /// <param name="images">The image handling logic.</param>
    public ContractsController(IContractService contracts, IImageService images)
    {
        _contracts = contracts;
        _images = images;
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
    [HttpPost("new/image")]
    public async Task<ActionResult<string>> UploadImageAsync(IFormFile file)
    {
        return await _images.TryStoreAsync(file.OpenReadStream());
    }
}
