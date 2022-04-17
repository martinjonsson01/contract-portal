namespace Application.Documents;

/// <summary>
///     A way of storing and loading documents.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    ///     Stores a document.
    /// </summary>
    /// <param name="stream">The stream of the document to store.</param>
    /// <returns>The identifier of the stored document.</returns>
    Task<string> StoreAsync(Stream stream);
}
