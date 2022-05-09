using Domain.Contracts;

namespace Application.FavoriteContracts;

/// <summary>
/// Used for interacting with favorite logic.
/// </summary>
public interface IFavoriteContractService
{
    /// <summary>
    /// Gets all contracts marked as favorite by a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <returns>All contracts marked as favorite by the user.</returns>
    IEnumerable<Contract> FetchAll(string userName);

    /// <summary>
    /// Checks if the contract is marked as favorite by the user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>Whether the contract was marked as favorite.</returns>
    bool IsFavorite(string userName, Guid contractId);

    /// <summary>
    /// Adds a favorite contract for a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    void Add(string userName, Guid contractId);

    /// <summary>
    /// Removes a favorite contract for a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>Whether the removal was successful.</returns>
    bool Remove(string userName, Guid contractId);
}
