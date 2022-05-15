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
    /// <param name="username">The username of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IList<RecentlyViewedContract> FetchRecentContracts(string username);

    /// <summary>
    /// Adds a contract to recent for a user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="contract">The recent contract.</param>
    void Add(string username, Contract contract);

    /// <summary>
    /// Removes the last viewed contract for user.
    /// </summary>
    /// <param name="contractToRemove">The contract to remove.</param>
    void Remove(RecentlyViewedContract contractToRemove);
}
