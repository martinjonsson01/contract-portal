using MimeDetective.InMemory;

namespace Infrastructure.Images;

/// <summary>
/// Verifies images before allowing them to pass through to the repository.
/// </summary>
public static class ImageVerifier
{
    /// <summary>
    /// Verifies if a binary data stream is a valid image that can safely be stored on the server.
    /// </summary>
    /// <param name="imageStream">The data stream to verify.</param>
    /// <returns>Whether the stream is a valid image.</returns>
    public static bool IsValid(Stream imageStream)
    {
        FileType fileType = imageStream.DetectMimeType();

        // Allow MIME-types starting with image/ since all widely used image formats do.
        return fileType.Mime.StartsWith("image/", StringComparison.InvariantCulture);
    }
}
