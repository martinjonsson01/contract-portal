using Domain.Contracts;
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
    /// Checks whether a user exists or not.
    /// </summary>
    /// <param name="username">The username to look for.</param>
    /// <returns>Whether the user exits or not.</returns>
    bool Exists(string username);

    /// <summary>
    /// Fetches recent view contracts for the user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The recently view contracts.</returns>
    IEnumerable<Contract> RecentlyViewed(Guid id);
}
