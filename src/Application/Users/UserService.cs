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
    public bool Remove(Guid id)
    {
        return _repo.Remove(id);
    }

    /// <inheritdoc />
    public bool ValidPassword(Guid id, string password)
    {
        return password.Equals("Password", StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public IEnumerable<User> FetchAllUsers()
    {
        return _repo.All;
    }
}
