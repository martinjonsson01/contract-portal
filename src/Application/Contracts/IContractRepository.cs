using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Logic for fetching and storing contracts.
/// </summary>
public interface IContractRepository
{
    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <returns>All currently existing contracts.</returns>
    IEnumerable<Contract> All { get; }

    /// <summary>
    /// Adds a new contract to store.
    /// </summary>
    /// <param name="contract">The new contract instance.</param>
    void Add(Contract contract);

    /// <summary>
    /// Removes the contract with the given ID.
    /// </summary>
    /// <param name="id">The id of the contract to be removed.</param>
    /// <returns>If the removal was successful.</returns>
    bool Remove(Guid id);

    /// <summary>
    /// Updates the contract in repository.
    /// </summary>
    /// <param name="updatedContract">The updated contract.</param>
    void UpdateContract(Contract updatedContract);

    /// <summary>
    /// Gets a contract with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the contract.</param>
    /// <returns>The contract, if it exists.</returns>
    Contract? FetchContract(Guid id);
}
