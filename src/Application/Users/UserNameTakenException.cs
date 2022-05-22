using Domain.Users;

namespace Application.Users;

/// <summary>
/// Thrown when the name of a <see cref="User"/> is already taken.
/// </summary>
public class UserNameTakenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNameTakenException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public UserNameTakenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNameTakenException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UserNameTakenException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNameTakenException"/> class.
    /// </summary>
    public UserNameTakenException()
    {
    }
}
