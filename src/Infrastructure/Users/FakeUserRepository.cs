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
    ///  Creates a fake user called Ted.
    /// </summary>
    public FakeUserRepository()
    {
        _users = new List<User>
        {
            new()
            {
                Name = "Ted",
                Company = "SJ",
                LatestPaymentDate = DateTime.Today,
            },
        };
    }

    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(_users);

    /// <inheritdoc />
    public void Add(User user)
    {
        _users.Add(user);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _users.RemoveAll(o => o.Id == id) > 0;
    }
}
