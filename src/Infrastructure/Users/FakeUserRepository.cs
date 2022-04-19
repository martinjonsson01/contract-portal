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
    /// Constructor.
    /// </summary>
    public FakeContractRepository()
    {
        _users = new List<User>();
    }

    /// <inheritdoc />
    public void Add(User user)
    {
        _users.Add(user);
    }
}
