namespace Application.Images;

/// <summary>
/// Logic for working with images.
/// </summary>
public interface IImageService
{
    /// <summary>
    ///     Tries to store an image.
    /// </summary>
    /// <param name="imageStream">The stream of the image to store.</param>
    /// <returns>The identifier of the stored image.</returns>
    Task<string> TryStoreAsync(Stream imageStream);
}
