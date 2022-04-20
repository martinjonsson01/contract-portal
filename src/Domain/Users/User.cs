namespace Domain.Users;

/// <summary>
/// A user from a certain company.
/// </summary>
public class User
{
    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; } = "Inget namn";

    /// <summary>
    /// Gets or sets the company of the user.
    /// </summary>
    public string Company { get; set; } = "Inget företagsnamn";

    /// <summary>
    /// Gets the date when the user was created.
    /// </summary>
    public DateTime DateCreated { get; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the date of the latest payment by the user. It is set to 1/1/0001 12:00:00 AM by default.
    /// </summary>
    public DateTime LatestPaymentDate { get; set; }
}