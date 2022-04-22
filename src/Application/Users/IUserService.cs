using Domain.Users;

namespace Application.Users;

/// <summary>
/// Used for interacting with user logic.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="user">The new user.</param>
    void Add(User user);
}
