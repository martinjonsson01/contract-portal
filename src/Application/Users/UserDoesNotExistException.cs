namespace Application.Users;

/// <summary>
/// Thrown when a user is not found.
/// </summary>
public class UserDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDoesNotExistException"/> class.
    /// </summary>
    /// <param name="userId">The ID of the user that could not be found.</param>
    public UserDoesNotExistException(Guid userId)
        : base($"Could not find user with ID {userId}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDoesNotExistException"/> class.
    /// </summary>
    public UserDoesNotExistException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDoesNotExistException"/> class.
    /// </summary>
    /// <param name="userId">The ID of the user that could not be found.</param>
    /// <param name="innerException">The inner exception.</param>
    public UserDoesNotExistException(Guid userId, Exception innerException)
        : base($"Could not find user with ID {userId}", innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UserDoesNotExistException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public UserDoesNotExistException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
