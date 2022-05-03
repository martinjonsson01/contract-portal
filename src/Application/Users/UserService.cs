using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
    public AuthenticateResponse Authenticate(string username)
    {
        User? user = _repo.Fetch(username);

        if (user is null)
            throw new UserDoesNotExistException(username);

        string token = GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);
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

    private static byte[] GetSecretKey()
    {
        const string environmentVariableKey = "prodigo_portal_jwt_secret";
        string? jwtSecret = Environment.GetEnvironmentVariable(environmentVariableKey);

        return jwtSecret is null
            ? throw new ArgumentException("No environment variable defined for " + environmentVariableKey)
            : Encoding.UTF8.GetBytes(jwtSecret);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = GetSecretKey();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), }),
            Audience = _config["Jwt:Issuer"],
            Issuer = _config["Jwt:Issuer"],
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
