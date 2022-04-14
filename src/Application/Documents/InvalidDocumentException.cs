namespace Application.Documents;

/// <summary>
/// Thrown when an invalid document was input.
/// </summary>
public class InvalidDocumentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidDocumentException"/> class.
    /// </summary>
    public InvalidDocumentException()
        : base("Document is invalid.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidDocumentException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidDocumentException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidDocumentException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">An inner exception.</param>
    public InvalidDocumentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
