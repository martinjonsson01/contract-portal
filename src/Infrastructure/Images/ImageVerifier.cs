using System.Collections.Immutable;
using MimeDetective;
using MimeDetective.Definitions;
using MimeDetective.Definitions.Licensing;
using MimeDetective.Engine;
using MimeDetective.Storage;

namespace Infrastructure.Images;

/// <summary>
/// Verifies images before allowing them to pass through to the repository.
/// </summary>
internal class ImageVerifier : IVerifier
{
    private ContentInspector _inspector;

    /// <summary>
    /// Initializes the image verifier.
    /// </summary>
    public ImageVerifier()
    {
        ImmutableArray<Definition> allDefinitions =
            new ExhaustiveBuilder { UsageType = UsageType.PersonalNonCommercial, }.Build();
        _inspector = new ContentInspectorBuilder { Definitions = allDefinitions, }.Build();
    }

    /// <summary>
    /// Verifies if a binary data stream is a valid image that can safely be stored on the server.
    /// </summary>
    /// <param name="stream">The data stream to verify.</param>
    /// <returns>Whether the stream is a valid image.</returns>
    public bool IsValid(Stream stream)
    {
        stream.Position = 0;
        ImmutableArray<MimeTypeMatch> results = _inspector.Inspect(stream).ByMimeType();
        stream.Position = 0;

        bool anyMatchIsImage =
            results.Any(match => match.MimeType.StartsWith("image/", StringComparison.InvariantCulture));

        // Allow MIME-types starting with image/ since all widely used image formats do.
        return anyMatchIsImage;
    }

    /// <summary>
    /// Returns the file extension matching the type of data stream.
    /// </summary>
    /// <param name="stream">The stream to find the file type of.</param>
    /// <returns>The file extension of the stream file type.</returns>
    public string GetFileExtensionOf(Stream stream)
    {
        stream.Position = 0;
        ImmutableArray<DefinitionMatch> extensions = _inspector.Inspect(stream);
        stream.Position = 0;

        return extensions.ByFileExtension().First().Extension;
    }
}
