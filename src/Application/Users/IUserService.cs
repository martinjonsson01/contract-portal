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
    /// Removes a user.
    /// </summary>
    /// <param name="id">User to be removed.</param>
    /// <returns>Whether the removal was successful.</returns>
    bool Remove(Guid id);

    /// <summary>
    /// Validates that the given password is the password of the given user.
    /// </summary>
    /// <param name="id">ID of the user to validate against.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>Whether the password is valid for the user.</returns>
    bool ValidPassword(Guid id, string password);
}
