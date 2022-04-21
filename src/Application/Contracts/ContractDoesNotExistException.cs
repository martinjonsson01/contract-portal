namespace Application.Contracts;

/// <summary>
/// Thrown when a contract that is trying to be accessed does not exist.
/// </summary>
public class ContractDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContractDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message to print.</param>
    public ContractDoesNotExistException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractDoesNotExistException"/> class.
    /// </summary>
    public ContractDoesNotExistException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message to print.</param>
    /// <param name="innerException">The inner exception that was thrown.</param>
    public ContractDoesNotExistException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
