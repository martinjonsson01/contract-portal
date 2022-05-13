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
    void UpdateUser(User user);

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
    /// Checks whether a user exists or not.
    /// </summary>
    /// <param name="username">The username to look for.</param>
    /// <returns>Whether the user exits or not.</returns>
    bool UserExists(string username);

    /// <summary>
    /// Generates an authentication token for a given user, if valid.
    /// </summary>
    /// <param name="username">The name of the <see cref="User"/> to authenticate.</param>
    /// <param name="password">The password of the <see cref="User"/> to validate.</param>
    /// <returns>A response containing the generated token.</returns>
    AuthenticateResponse Authenticate(string username, string password);
}
