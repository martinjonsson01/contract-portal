using Application.Documents;

using Domain.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Web API for manipulating document files.
/// </summary>
[Route("api/v1/[controller]")]
public class DocumentsController : BaseApiController<DocumentsController>
{
    private readonly IDocumentService _documents;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentsController"/> class.
    /// </summary>
    /// <param name="logger">The logging provider.</param>
    /// <param name="documents">A way of handling documents.</param>
    public DocumentsController(ILogger<DocumentsController> logger, IDocumentService documents)
        : base(logger)
    {
        _documents = documents;
    }

    /// <summary>
    /// Uploads a new document.
    /// </summary>
    /// <returns>The stored document.</returns>
    /// <response code="400">The uploaded file is not a valid document.</response>
    [HttpPost]
    [Authorize("AdminOnly")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Document>> UploadDocumentAsync()
    {
        IFormFile file = Request.Form.Files[0];
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        Logger.LogInformation("Trying to upload a document file: {Name}", file.FileName);
        try
        {
            Document document = await _documents.StoreAsync(file.FileName, file.OpenReadStream()).ConfigureAwait(false);

            // Update path to match controller API endpoint.
            document.Path = $"/documents/{document.Path}";
            return Ok(document);
        }
        catch (InvalidDocumentException e)
        {
            Logger.LogInformation("Error occured during document upload: {Error}", e.Message);
            return BadRequest();
        }
    }
}
