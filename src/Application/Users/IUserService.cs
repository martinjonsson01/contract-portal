using Domain.Contracts;
using Domain.Users;

namespace Application.Users;

/// <summary>
/// Used for interacting with user logic.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All users.</returns>
    IEnumerable<User> FetchAllUsers();

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="user">The new user.</param>
    void Add(User user);

    /// <summary>
    /// Updates the user with the new values.
    /// </summary>
    /// <param name="user">The new user.</param>
    /// <param name="encryptPassword">Whether to encrypt the password-field of the user.</param>
    void UpdateUser(User user, bool encryptPassword = true);

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <param name="id">The id of the user to fetch.</param>
    /// <returns>The fetched user.</returns>
    User FetchUser(Guid id);

    /// <summary>
    /// Removes a user.
    /// </summary>
    /// <param name="id">User to be removed.</param>
    /// <returns>Whether the removal was successful.</returns>
    bool Remove(Guid id);

    /// <summary>
    /// Generates an authentication token for a given user, if valid.
    /// </summary>
    /// <param name="username">The name of the <see cref="User"/> to authenticate.</param>
    /// <param name="password">The password of the <see cref="User"/> to validate.</param>
    /// <returns>A response containing the generated token.</returns>
    AuthenticateResponse Authenticate(string username, string password);

    /// <summary>
    /// Gets all contracts marked as favorite by a certain user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>All contracts marked as favorite by the user.</returns>
    IEnumerable<Contract> FetchAllFavorites(Guid userId);

    /// <summary>
    /// Checks if the contract is marked as favorite by the user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="contractId">The ID of the contract.</param>
    /// <returns>Whether the contract was marked as favorite.</returns>
    bool IsFavorite(Guid userId, Guid contractId);

    /// <summary>
    /// Adds a favorite contract for a certain user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="contractId">The ID of the contract.</param>
    void AddFavorite(Guid userId, Guid contractId);

    /// <summary>
    /// Removes a favorite contract for a certain user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="contractId">The ID of the contract.</param>
    /// <returns>Whether the removal was successful.</returns>
    bool RemoveFavorite(Guid userId, Guid contractId);
}
