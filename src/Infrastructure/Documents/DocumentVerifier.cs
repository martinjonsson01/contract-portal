using System.Collections.Immutable;
using System.Text.RegularExpressions;
using MimeDetective;
using MimeDetective.Definitions;
using MimeDetective.Engine;

namespace Infrastructure.Documents;

/// <summary>
/// Verifies documents before allowing them to pass through to the repository.
/// </summary>
internal class DocumentVerifier : IVerifier
{
    private ContentInspector _inspector;

    /// <summary>
    /// Initializes the document verifier.
    /// </summary>
    public DocumentVerifier()
    {
        _inspector = new ContentInspectorBuilder { Definitions = Default.All(), }.Build();
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

        // Allow the following MIME-types:
        // word, pdf, excel, png, jpg, gif, jpeg
        string pattern =
            @"( (application) +\/ (msword | pdf | vnd.openxmlformats-officedocument.spreadsheetml.sheet | vnd.ms-excel )  | (image) +\/ (png | jpg | gif | jpeg))";
        var rgDoc = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

        return results.Any(match => rgDoc.IsMatch(match.MimeType));
    }

    /// <summary>
    /// Returns the file extension matching the type of data stream.
    /// </summary>
    /// <param name="stream">The stream to find the file type of.</param>
    /// <returns>The file extension of the stream file type.</returns>
    public string GetFileExtensionOf(Stream stream)
    {
        stream.Position = 0;
        ImmutableArray<FileExtensionMatch> extensions = _inspector.Inspect(stream).ByFileExtension();
        stream.Position = 0;

        return extensions.First().Extension;
    }
}
