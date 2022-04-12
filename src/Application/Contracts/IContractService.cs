using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Used for interacting with contract logic.
/// </summary>
public interface IContractService
{
    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <returns>All contracts.</returns>
    IEnumerable<Contract> FetchAllContracts();

    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <returns>Top most recently viewed contracts.</returns>
    IEnumerable<Contract> FetchRecentContracts();

    /// <summary>
    /// Adds a new contract.
    /// </summary>
    /// <param name="contract">The new contract.</param>
    void Add(Contract contract);

    /// <summary>
    /// Removes the specified contract.
    /// </summary>
    /// <param name="id">The id of the contract to be removed.</param>
    /// <returns>Whether the removal was successful.</returns>
    bool Remove(Guid id);
}
