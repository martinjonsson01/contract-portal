using Domain.Contracts;

namespace Application.FavoriteContracts;

/// <summary>
/// Logic for fetching and storing users.
/// </summary>
public interface IFavoriteContractRepository
{
    /// <summary>
    /// Checks if the contract is marked as favorite by the user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>Whether the contract was marked as favorite.</returns>
    bool CheckIfFavorite(string userName, Guid contractId);

    /// <summary>
    /// Fetches all contracts marked as favorite.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <returns>All contracts marked as favorite.</returns>
    IEnumerable<Contract> FetchAllFavorites(string userName);

    /// <summary>
    /// Adds a new favorite to store.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    void Add(string userName, Guid contractId);

    /// <summary>
    /// Removes the user with the given ID.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>If the removal was successful.</returns>
    bool Remove(string userName, Guid contractId);
}
