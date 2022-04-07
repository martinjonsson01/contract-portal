namespace Application.Images;

/// <summary>
/// Verifies images before allowing them to pass through to the repository.
/// </summary>
public class ImageVerifier : IImageService
{
    private readonly IImageRepository _repository;

    /// <summary>
    /// Constructs an image verifier.
    /// </summary>
    /// <param name="repository">Where to store verified images.</param>
    public ImageVerifier(IImageRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Checks if an image is valid and stores it.
    /// </summary>
    /// <param name="imageStream">The image to store.</param>
    /// <returns>The identifier of the stored image.</returns>
    public async Task<string> TryStoreAsync(Stream imageStream)
    {
        if (imageStream is null)
            throw new ArgumentNullException(nameof(imageStream));

        bool isValid = await IsValidAsync(imageStream).ConfigureAwait(false);
        return isValid
            ? await _repository.StoreAsync(imageStream).ConfigureAwait(false)
            : throw new InvalidImageException();
    }

    private static Task<bool> IsValidAsync(Stream imageStream)
    {
        // Very fundamental verification, basically no security. Important to add real verification here
        // in the future.
        return Task.FromResult(imageStream.CanRead);
    }
}
