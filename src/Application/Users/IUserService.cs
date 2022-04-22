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
    /// Searches for and returns users that match the given query.
    /// </summary>
    /// <param name="query">The query to search for.</param>
    /// <returns>The users found.</returns>
    IEnumerable<User> Search(string query);
}
