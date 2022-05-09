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
    /// Checks whether the current session is authenticated.
    /// </summary>
    /// <returns>Whether the current session is authenticated.</returns>
    Task<bool> IsAuthenticatedAsync();

    /// <summary>
    /// Begins a new authenticated user session.
    /// </summary>
    /// <param name="authentication">The authentication used to identify the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BeginAsync(AuthenticateResponse authentication);

    /// <summary>
    /// Tries to resume a user session from the already available data.
    /// </summary>
    /// <returns>The task result contains the authentication state, if it succeeded.</returns>
    Task<AuthenticateResponse?> TryResumeAsync();
}
