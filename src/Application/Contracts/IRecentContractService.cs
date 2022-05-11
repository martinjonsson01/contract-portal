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
    /// <param name="id">The id of the user.</param>
    /// <returns>The number of current recent contracts.</returns>
    int Size(string id);

    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IEnumerable<Contract> FetchRecentContracts(string id);

    /// <summary>
    /// Ensures that a new contract is qualified as recently viewed.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void Add(string id, Contract contract);
}
