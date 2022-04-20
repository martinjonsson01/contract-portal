using Application.Users;
using Domain.Users;

namespace Infrastructure.Users;

/// <summary>
/// Mocks fake users.
/// </summary>
public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users;

    /// <summary>
    /// Constructs a FakeUserRepository.
    /// </summary>
    public FakeUserRepository()
    {
        _users = new List<User>();
    }

    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(_users);

    /// <inheritdoc />
    public void Add(User user)
    {
        _users.Add(user);
    }
}
