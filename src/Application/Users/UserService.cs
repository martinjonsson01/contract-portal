using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Configuration;
using Application.Exceptions;

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

        _repo.Add(user);
    }

    /// <inheritdoc />
    public bool UserExists(string username)
    {
        return _repo.Exists(username);
    }

    /// <inheritdoc />
    public AuthenticateResponse Authenticate(string username, string password)
    {
        User? user = _repo.Fetch(username);

        return user is null
            ? throw new UserDoesNotExistException(username)
            : ValidPassword(user, password)
                ? new AuthenticateResponse(user, GenerateJwtToken(user))
                : throw new InvalidPasswordException("Wrong password.");
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

    private static IEnumerable<Claim> CreateClaims(User user)
    {
        var claims = new List<Claim> { new("id", user.Id.ToString()), };

        if (user.Name == "admin")
            claims.Add(new Claim("IsAdmin", "true"));

        return claims;
    }

    private static bool ValidPassword(User user, string password)
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
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
