using System.Text.Json.Serialization;

using Domain.Users;

namespace Application.Users;

/// <summary>
/// A response returned by the server when trying to authenticate a user.
/// </summary>
public class AuthenticateResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateResponse"/> class.
    /// </summary>
    /// <param name="id">The identifier of the user being authenticated.</param>
    /// <param name="username">The name of the user being authenticated.</param>
    /// <param name="token">The token of the user.</param>
    [JsonConstructor]
    public AuthenticateResponse(Guid id, string username, string token)
    {
        Id = id;
        Username = username;
        Token = token;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateResponse"/> class.
    /// </summary>
    /// <param name="user">The user being authenticated.</param>
    /// <param name="token">The token of the user.</param>
    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Username = user.Name;
        Token = token;
    }

    /// <summary>
    /// Gets or sets the identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the token of the user.
    /// </summary>
    public string Token { get; set; }
}
