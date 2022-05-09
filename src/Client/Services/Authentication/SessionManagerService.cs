using Application.Users;

using Blazored.SessionStorage;

namespace Client.Services.Authentication;

/// <summary>
/// Manages the session by storing it in the local session storage of the browser.
/// </summary>
internal class SessionManagerService : ISessionService
{
    private readonly ISessionStorageService _storage;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionManagerService"/> class.
    /// </summary>
    /// <param name="storage">A way to store session data.</param>
    public SessionManagerService(ISessionStorageService storage)
    {
        _storage = storage;
    }

    /// <inheritdoc/>
    public event EventHandler<AuthenticationEventArgs>? AuthenticationStateChanged;

    /// <inheritdoc/>
    public Task StartAsync(AuthenticateResponse authentication)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<AuthenticateResponse?> TryResumeAsync()
    {
        bool loggedIn = await _storage.ContainKeyAsync("user").ConfigureAwait(true);
        if (!loggedIn)
#pragma warning disable RETURN0001
            return null;
#pragma warning restore RETURN0001

        AuthenticateResponse state = await _storage.GetItemAsync<AuthenticateResponse>("user").ConfigureAwait(true);
        AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs { State = state, });
        return state;
    }
}
