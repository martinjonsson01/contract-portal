namespace Application.Users;

/// <summary>
/// Thrown when a user is not found.
/// </summary>
public class UserDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDoesNotExistException"/> class.
    /// </summary>
    /// <param name="username">The name of the user that could not be found.</param>
    public UserDoesNotExistException(string username)
        : base($"Could not find user with name {username}")
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
    /// <param name="username">The name of the user that could not be found.</param>
    /// <param name="innerException">The inner exception.</param>
    public UserDoesNotExistException(string username, Exception innerException)
        : base($"Could not find user with name {username}", innerException)
    {
    }
}
