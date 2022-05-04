using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Logic for fetching and storing recent contract.
/// </summary>
public interface IRecentContractRepository
{
    /// <summary>
    /// Gets how many recent contracts there  for a user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The number of current recent contracts.</returns>
    int Size(Guid id);

    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IEnumerable<Contract> FetchRecentContracts(Guid id);

    /// <summary>
    /// Ensures that a new contract is qualified as recently viewed for a user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void Add(Guid id, Contract contract);

    /// <summary>
    /// Removes the oldest recently viewed contract.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    void RemoveLast(Guid id);

    /// <summary>
    /// Remove contract from all users recent contracts.
    /// </summary>
    /// <param name="id">The if of the contract to remove.</param>
    void Remove(Guid id);
}
