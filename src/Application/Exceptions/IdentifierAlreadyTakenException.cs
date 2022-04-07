namespace Application.Exceptions;

/// <summary>
/// The identifier was already taken.
/// </summary>
public class IdentifierAlreadyTakenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierAlreadyTakenException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public IdentifierAlreadyTakenException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierAlreadyTakenException"/> class.
    /// </summary>
    public IdentifierAlreadyTakenException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierAlreadyTakenException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public IdentifierAlreadyTakenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
