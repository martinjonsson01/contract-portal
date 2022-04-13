namespace Application.Documents;

/// <summary>
///     A way of storing and loading images.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    ///     Stores an image.
    /// </summary>
    /// <param name="imageStream">The stream of the image to store.</param>
    /// <returns>The identifier of the stored image.</returns>
    Task<string> StoreAsync(Stream imageStream);
}
