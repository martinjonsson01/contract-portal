using Application.Documents;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Web API for manipulating document files.
/// </summary>
[Route("api/v1/[controller]")]
public class DocumentsController : BaseApiController<DocumentsController>
{
    private readonly IDocumentRepository _documents;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentsController"/> class.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="documents">The place to store document.</param>
    public DocumentsController(ILogger<DocumentsController> logger, IDocumentRepository documents)
        : base(logger)
    {
        _documents = documents;
    }

    /// <summary>
    /// Uploads a new document.
    /// </summary>
    /// <returns>The identifier of the stored document.</returns>
    /// <response code="400">The uploaded file is not a valid document.</response>
    [HttpPost]
    [Produces("text/plain")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadDocumentAsync()
    {
        IFormFile file = Request.Form.Files[0];
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        Logger.LogInformation("Trying to upload a document file: {Name}", file.Name);
        try
        {
            string documentName = await _documents.StoreAsync(file.OpenReadStream()).ConfigureAwait(false);
            return Ok($"/documents/{documentName}");
        }
        catch (InvalidDocumentException e)
        {
            Logger.LogInformation("Error occured during document upload: {Error}", e.Message);
            return BadRequest();
        }
    }
}
