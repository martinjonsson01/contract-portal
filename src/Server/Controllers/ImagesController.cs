using Application.Images;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Web API for manipulating image files.
/// </summary>
[Route("api/v1/[controller]")]
public class ImagesController : BaseApiController<ImagesController>
{
    private readonly IImageRepository _images;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImagesController"/> class.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="images">The place to store images.</param>
    public ImagesController(ILogger<ImagesController> logger, IImageRepository images)
        : base(logger)
    {
        _images = images;
    }

    /// <summary>
    /// Uploads a new contract image file.
    /// </summary>
    /// <returns>The identifier of the stored image.</returns>
    /// <response code="400">The uploaded file is not a valid image.</response>
    [HttpPost]
    [Produces("text/plain")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadImageAsync()
    {
        IFormFile file = Request.Form.Files[0];
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        Logger.LogInformation("Trying to upload an image file: {Name}", file.Name);
        try
        {
            string imageName = await _images.StoreAsync(file.OpenReadStream()).ConfigureAwait(false);
            return Ok($"/images/{imageName}");
        }
        catch (InvalidImageException e)
        {
            Logger.LogInformation("Error occured during image upload: {Error}", e.Message);
            return BadRequest();
        }
    }
}
