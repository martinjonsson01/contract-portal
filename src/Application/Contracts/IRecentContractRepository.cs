using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Logic for fetching and storing recent contracts.
/// </summary>
public interface IRecentContractRepository
{
    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IList<RecentlyViewedContract> FetchRecentContracts(Guid userId);

    /// <summary>
    /// Marks a contract as recently viewed for a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void Add(Guid userId, Contract contract);

    /// <summary>
    /// Removes the last viewed contract for user.
    /// </summary>
    /// <param name="contractToRemove">The contract to remove.</param>
    void Remove(RecentlyViewedContract contractToRemove);
}
