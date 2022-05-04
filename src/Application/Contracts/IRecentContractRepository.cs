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
    Queue<Contract> FetchRecentContracts(string id);

    /// <summary>
    /// Updates the recent contracts for a user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <param name="updatedRecentContracts">The updated recent contracts.</param>
    void UpdateRecentContracts(string id, Queue<Contract> updatedRecentContracts);
}
