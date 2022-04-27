using Application.Users;
using Domain.Users;

namespace Infrastructure.Users;

/// <summary>
/// Mocks fake users.
/// </summary>
public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new() { new User() { Name = "user1" } };

    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(_users);

    /// <inheritdoc />
    public void Add(User user)
    {
        _users.Add(user);
    }

    /// <inheritdoc />
    public bool UserExists(string username)
    {
        return _users.Find(user => user.Name == username) != null;
    }
}
