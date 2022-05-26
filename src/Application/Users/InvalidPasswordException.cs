namespace Application.Users;

/// <summary>
/// Thrown when a given password does not belong to a user.
/// </summary>
public class InvalidPasswordException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPasswordException"/> class.
    /// </summary>
    /// <param name="message">Specified error message.</param>
    public InvalidPasswordException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPasswordException"/> class.
    /// </summary>
    public InvalidPasswordException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPasswordException"/> class.
    /// </summary>
    /// <param name="message">Specified error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidPasswordException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
