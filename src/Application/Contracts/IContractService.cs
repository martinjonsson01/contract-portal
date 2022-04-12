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
    /// Gets the three most recently viewed contracts.
    /// </summary>
    /// <returns>Three contracts.</returns>
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
