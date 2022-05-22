using Application.Users;

namespace Client.Services.Authentication;

/// <summary>
/// Manages the session of the current user.
/// </summary>
public interface ISessionService
{
    /// <summary>
    /// Called whenever there are changes to the authentication of the user in the current session.
    /// </summary>
    event EventHandler<AuthenticationEventArgs> AuthenticationStateChanged;

    /// <summary>
    /// Gets a value indicating whether the current session is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the name of the currently authenticated user, if authenticated.
    /// </summary>
    string? Username { get; }

    /// <summary>
    /// Gets the ID of the currently authenticated user, if authenticated.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Begins a new authenticated user session.
    /// </summary>
    /// <param name="authentication">The authentication used to identify the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BeginAsync(AuthenticateResponse authentication);

    /// <summary>
    /// Ends the current authenticated user session.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EndAsync();

    /// <summary>
    /// Tries to resume a user session from the already available data.
    /// </summary>
    /// <returns>The task result contains the whether it succeeded.</returns>
    Task<bool> TryResumeAsync();
}
