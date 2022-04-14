using System.Text.RegularExpressions;
using MimeDetective.InMemory;

namespace Infrastructure.Documents;

/// <summary>
/// Verifies documents before allowing them to pass through to the repository.
/// </summary>
internal class DocumentVerifier : IVerifier
{
    /// <summary>
    /// Verifies if a binary data stream is a valid document that can safely be stored on the server.
    /// </summary>
    /// <param name="stream">The data stream to verify.</param>
    /// <returns>Whether the stream is a valid document.</returns>
    public bool IsValid(Stream stream)
    {
        // Allow the following MIME-types:
        // word, pdf, excel, png, jpg, gif, jpeg
        string pattern = @"( (application) +\/ (msword | pdf | vnd.openxmlformats-officedocument.spreadsheetml.sheet | vnd.ms-excel )  | (image) +\/ (png | jpg | gif | jpeg))";

        FileType fileType = stream.DetectMimeType();
        var rgDoc = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

        return rgDoc.IsMatch(fileType.Mime);
    }
}
