using Application.Users;

namespace Client.Services.Authentication;

/// <summary>
/// Contains information about an authentication changed event.
/// </summary>
public class AuthenticationEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the authentication state associated with this event.
    /// </summary>
    public AuthenticateResponse? State { get; set; }
}
