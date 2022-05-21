using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Configuration;
using Application.Exceptions;

using Domain.Contracts;
using Domain.Users;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Users;

/// <inheritdoc />
public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IConfiguration _config;

    /// <summary>
    /// Constructs user service.
    /// </summary>
    /// <param name="repo">Where to store and fetch users from.</param>
    /// <param name="config">The configuration source.</param>
    public UserService(IUserRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;

        _repo.EnsureAdminCreated();
    }

    /// <inheritdoc />
    public void Add(User user)
    {
        if (_repo.All.Any(otherUser => user.Id.Equals(otherUser.Id)))
            throw new IdentifierAlreadyTakenException();

        if (_repo.All.Any(otherUser => user.Name.Equals(otherUser.Name, StringComparison.Ordinal)))
            throw new UserNameTakenException();

        EncryptPasswordOf(user);
        _repo.Add(user);
    }

    /// <inheritdoc />
    public AuthenticateResponse Authenticate(string username, string password)
    {
        User? user = _repo.FromName(username);

        return user is null
            ? throw new UserDoesNotExistException(username)
            : IsPasswordValid(user, password)
                ? new AuthenticateResponse(user, GenerateJwtToken(user))
                : throw new InvalidPasswordException();
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _repo.Remove(id);
    }

    /// <inheritdoc />
    public IEnumerable<User> FetchAllUsers()
    {
        return _repo.All;
    }

    /// <inheritdoc />
    public void UpdateUser(User user)
    {
        EncryptPasswordOf(user);
        _repo.UpdateUser(user);
    }

    /// <inheritdoc />
    public User FetchUser(Guid id)
    {
        return _repo.Fetch(id) ?? throw new UserDoesNotExistException();
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchAllFavorites(Guid userId)
    {
        return FetchUser(userId).Favorites;
    }

    /// <inheritdoc />
    public bool IsFavorite(Guid userId, Guid contractId)
    {
        return FetchUser(userId).Favorites.Any(c => c.Id == contractId);
    }

    /// <inheritdoc />
    public void AddFavorite(Guid userId, Guid contractId)
    {
        _repo.AddFavorite(userId, contractId);
    }

    /// <inheritdoc />
    public bool RemoveFavorite(Guid userId, Guid contractId)
    {
        return _repo.RemoveFavorite(userId, contractId);
    }

    private static IEnumerable<Claim> CreateClaims(User user)
    {
        var claims = new List<Claim> { new("id", user.Id.ToString()), };

        if (user.Name == "admin")
            claims.Add(new Claim("IsAdmin", "true"));

        return claims;
    }

    private static void EncryptPasswordOf(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
    }

    private static bool IsPasswordValid(User user, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_config[ConfigurationKeys.JwtSecret]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(CreateClaims(user)),
            Audience = _config[ConfigurationKeys.JwtIssuer],
            Issuer = _config[ConfigurationKeys.JwtIssuer],
            Expires = DateTime.Now.AddMonths(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
