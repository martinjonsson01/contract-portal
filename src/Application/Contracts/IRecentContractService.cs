using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Methods for recent contracts.
/// </summary>
public interface IRecentContractService
{
    /// <summary>
    /// Gets how many recent contracts there are for given user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The number of current recent contracts.</returns>
    int Size(string username);

    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <param name="username">The id of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IEnumerable<Contract> FetchRecentContracts(string username);

    /// <summary>
    /// Ensures that a new contract is qualified as recently viewed.
    /// </summary>
    /// <param name="username">The id of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void Add(string username, Contract contract);
}
