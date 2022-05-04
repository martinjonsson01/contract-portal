using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Logic for fetching and storing recent contract.
/// </summary>
public interface IRecentContractRepository
{
    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    LinkedList<Contract> FetchRecentContracts(string id);

    /// <summary>
    /// Adds a contract to recent for a user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void Add(string id, Contract contract);

    /// <summary>
    /// Removes the contract from recent for all users.
    /// </summary>
    /// <param name="id">The ID of the contract to remove.</param>
    void Remove(Guid id);

    /// <summary>
    /// Removes the last viewed contract for user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    void RemoveLast(string id);
}
