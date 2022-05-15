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
    /// <param name="username">The username of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IList<RecentlyViewedContract> FetchRecentContracts(string username);

    /// <summary>
    /// Marks a contract as recently viewed for a user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void AddRecent(string username, Contract contract);

    /// <summary>
    /// Removes the last viewed contract for user.
    /// </summary>
    /// <param name="contractToRemove">The contract to remove.</param>
    void RemoveRecent(RecentlyViewedContract contractToRemove);
}
