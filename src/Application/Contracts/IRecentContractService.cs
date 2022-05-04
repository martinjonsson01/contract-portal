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

    /// <summary>
    /// Removes the contract with the specified ID if it exists.
    /// </summary>
    /// <param name="id">The ID of the contract to remove.</param>
    void Remove(Guid id);

    /// <summary>
    /// Removes the last viewed contract.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    void RemoveLast(string id);
}
