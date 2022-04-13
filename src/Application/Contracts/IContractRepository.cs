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
    public bool Remove(Guid id);

    /// <summary>
    /// Gets all contracts marked as favorites.
    /// </summary>
    /// <returns>The contract with a favorite mark.</returns>
    IEnumerable<Contract> Favorites();

    /// <summary>
    /// Updates the contract's favorite status.
    /// </summary>
    /// <param name="id">The id of the contract to update.</param>
    void UpdateFavorite(Guid id);
}
