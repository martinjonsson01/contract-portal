using Domain.Users;

namespace Application.Users;

/// <summary>
/// Logic for fetching and storing users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All currently existing users.</returns>
    IEnumerable<User> All { get; }

    /// <summary>
    /// Adds a new user to store.
    /// </summary>
    /// <param name="user">The new user instance.</param>
    void Add(User user);

    /// <summary>
    /// Removes the user with the given ID.
    /// </summary>
    /// <param name="id">The id of the user to be removed.</param>
    /// <returns>If the removal was successful.</returns>
    bool Remove(Guid id);

    /// <summary>
    /// Gets the user with the given username if it exists.
    /// </summary>
    /// <param name="username">The name of the user to get.</param>
    /// <returns>The user, if it exists.</returns>
    User? Fetch(string username);

    /// <summary>
    /// Checks whether a user exists or not.
    /// </summary>
    /// <param name="username">The username to look for.</param>
    /// <returns>Whether the user exits or not.</returns>
    bool Exists(string username);

    /// <summary>
    /// Ensures that an admin user is created.
    /// </summary>
    void EnsureAdminCreated();

    /// <summary>
    /// Gets a user with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The user, if it exists.</returns>
    User? FetchUser(Guid id);

    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <param name="updatedUser">The updated user.</param>
    void UpdateUser(User updatedUser);

    /// <summary>
    /// Adds a favorite contract to store, for a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    void AddFavorite(string userName, Guid contractId);

    /// <summary>
    /// Removes a stored favorite contract, for a certain user.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>If the removal was successful.</returns>
    bool RemoveFavorite(string userName, Guid contractId);
}
