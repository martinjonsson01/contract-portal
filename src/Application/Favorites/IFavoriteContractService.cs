using Domain.Contracts;

namespace Application.FavoriteContracts;

/// <summary>
/// Used for interacting with user logic.
/// </summary>
public interface IFavoriteContractService
{
    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <returns>All users.</returns>
    IEnumerable<Contract> FetchAllFavorites(string userName);

    /// <summary>
    /// Checks if the contract is marked as favorite by the user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>Whether the contract was marked as favorite.</returns>
    bool CheckIfFavorite(string userName, Guid contractId);

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    void Add(string userName, Guid contractId);

    /// <summary>
    /// Removes a user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>Whether the removal was successful.</returns>
    bool Remove(string userName, Guid contractId);
}
