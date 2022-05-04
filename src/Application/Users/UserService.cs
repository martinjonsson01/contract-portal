using Application.Exceptions;
using Domain.Users;

namespace Application.Users;

/// <inheritdoc />
public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    /// <summary>
    /// Constructs user service.
    /// </summary>
    /// <param name="repo">Where to store and fetch users from.</param>
    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public void Add(User user)
    {
        if (_repo.All.Any(otherUser => user.Id.Equals(otherUser.Id)))
            throw new IdentifierAlreadyTakenException();

        _repo.Add(user);
    }

    /// <inheritdoc />
    public bool UserExists(string username)
    {
        return _repo.Exists(username);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _repo.Remove(id);
    }

    /// <inheritdoc />
    public bool ValidPassword(string username, string password)
    {
        string? userPassword = _repo.Fetch(username)?.Password;
        return BCrypt.Net.BCrypt.Verify(password, userPassword);
    }

    /// <inheritdoc />
    public IEnumerable<User> FetchAllUsers()
    {
        return _repo.All;
    }
}
