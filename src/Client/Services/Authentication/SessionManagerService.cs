using System.Net.Http.Headers;

using Application.Users;

using Blazor.Analytics;

using Blazored.SessionStorage;

namespace Client.Services.Authentication;

/// <summary>
/// Manages the session by storing it in the local session storage of the browser.
/// </summary>
internal class SessionManagerService : ISessionService
{
    private readonly ISessionStorageService _storage;
    private readonly HttpClient _http;
    private readonly IAnalytics _analytics;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionManagerService"/> class.
    /// </summary>
    /// <param name="storage">A way to store session data.</param>
    /// <param name="http">The client to add authentication tokens to.</param>
    /// <param name="analytics">A way of tracking analytics data.</param>
    public SessionManagerService(ISessionStorageService storage, HttpClient http, IAnalytics analytics)
    {
        _storage = storage;
        _http = http;
        _analytics = analytics;
    }

    /// <inheritdoc/>
    public event EventHandler<AuthenticationEventArgs>? AuthenticationStateChanged;

    /// <inheritdoc/>
    public bool IsAuthenticated { get; private set; }

    /// <inheritdoc/>
    public string? Username { get; private set; }

    /// <inheritdoc/>
    public Guid? UserId { get; private set; }

    /// <inheritdoc/>
    public async Task BeginAsync(AuthenticateResponse authentication)
    {
        await _storage.SetItemAsync("user", authentication).ConfigureAwait(true);

        Authenticate(authentication);
    }

    /// <inheritdoc/>
    public async Task EndAsync()
    {
        await _storage.RemoveItemAsync("user").ConfigureAwait(true);

        _http.DefaultRequestHeaders.Authorization = null;

        IsAuthenticated = false;
        Username = null;
        UserId = null;

        AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs { State = null, });
    }

    /// <inheritdoc/>
    public async Task<bool> TryResumeAsync()
    {
        IsAuthenticated = await _storage.ContainKeyAsync("user").ConfigureAwait(true);
        if (!IsAuthenticated)
        {
            AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs { State = null, });
            return false;
        }

        AuthenticateResponse state = await _storage.GetItemAsync<AuthenticateResponse>("user").ConfigureAwait(true);
        Authenticate(state);
        return true;
    }

    private void Authenticate(AuthenticateResponse authentication)
    {
        // Store token on http client so all future requests use the token.
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authentication.Token);

        IsAuthenticated = true;
        Username = authentication.Username;
        UserId = authentication.Id;

        AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs { State = authentication, });
        _analytics.TrackEvent("user_authenticated", new { name = Username, id = UserId, });
    }
}
