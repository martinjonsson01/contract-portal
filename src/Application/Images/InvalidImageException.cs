namespace Application.Images;

/// <summary>
/// Thrown when an invalid image was input.
/// </summary>
public class InvalidImageException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidImageException"/> class.
    /// </summary>
    public InvalidImageException()
        : base("Image is invalid.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidImageException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidImageException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidImageException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">An inner exception.</param>
    public InvalidImageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
