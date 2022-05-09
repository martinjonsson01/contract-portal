using Domain.Contracts;

namespace Application.FavoriteContracts;

/// <summary>
/// Logic for fetching and storing favorites.
/// </summary>
public interface IFavoriteContractRepository
{
    /// <summary>
    /// Adds a favorite contract to store, for a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    void Add(string userName, Guid contractId);

    /// <summary>
    /// Removes a stored favorite contract, for a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>If the removal was successful.</returns>
    bool Remove(string userName, Guid contractId);
}
