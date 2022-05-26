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
    /// Adds a new <see cref="User"/> to store.
    /// </summary>
    /// <param name="user">The new <see cref="User"/> instance.</param>
    void Add(User user);

    /// <summary>
    /// Removes the <see cref="User"/> with the given ID.
    /// </summary>
    /// <param name="id">The id of the <see cref="User"/> to be removed.</param>
    /// <returns>If the removal was successful.</returns>
    bool Remove(Guid id);

    /// <summary>
    /// Gets a <see cref="User"/> with the given ID, if it exists.
    /// </summary>
    /// <param name="id">The ID of the <see cref="User"/> to get.</param>
    /// <returns>The <see cref="User"/>, if it exists.</returns>
    User? Fetch(Guid id);

    /// <summary>
    /// Gets a <see cref="User"/> with the given name, if it exists.
    /// </summary>
    /// <param name="username">The name of the <see cref="User"/> to get.</param>
    /// <returns>The <see cref="User"/>, if it exists.</returns>
    User? FromName(string username);

    /// <summary>
    /// Ensures that an admin <see cref="User"/> is created.
    /// </summary>
    void EnsureAdminCreated();

    /// <summary>
    /// Updates a <see cref="User"/>.
    /// </summary>
    /// <param name="updatedUser">The updated <see cref="User"/>.</param>
    void UpdateUser(User updatedUser);

    /// <summary>
    /// Adds a favorite contract to store, for a certain <see cref="User"/>.
    /// </summary>
    /// <param name="userId">The id of the <see cref="User"/>.</param>
    /// <param name="contractId">The id of the <see cref="Contract"/>.</param>
    void AddFavorite(Guid userId, Guid contractId);

    /// <summary>
    /// Removes a stored favorite contract, for a certain <see cref="User"/>.
    /// </summary>
    /// <param name="userId">The id of the <see cref="User"/>.</param>
    /// <param name="contractId">The id of the contract.</param>
    /// <returns>If the removal was successful.</returns>
    bool RemoveFavorite(Guid userId, Guid contractId);
}
