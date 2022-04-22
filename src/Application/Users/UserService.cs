using Application.Exceptions;
using Application.Search;
using Domain.Users;

namespace Application.Users;

/// <inheritdoc />
public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly SearchEngine<User> _search;

    /// <summary>
    /// Constructs user service.
    /// </summary>
    /// <param name="repo">Where to store and fetch users from.</param>
    /// <param name="search">The search engine to use when searching through users.</param>
    public UserService(IUserRepository repo, SearchEngine<User> search)
    {
        _repo = repo;
        _search = search;
    }

    /// <inheritdoc />
    public void Add(User user)
    {
        if (_repo.All.Any(otherUser => user.Id.Equals(otherUser.Id)))
            throw new IdentifierAlreadyTakenException();

        _repo.Add(user);
    }

    /// <inheritdoc />
    public IEnumerable<User> FetchAllUsers()
    {
        return _repo.All;
    }

    /// <inheritdoc />
    public IEnumerable<User> Search(string query)
    {
        return _search.Search(query, FetchAllUsers());
    }
}
