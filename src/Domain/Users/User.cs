using Domain.Contracts;

namespace Domain.Users;

/// <summary>
/// A user from a certain company.
/// </summary>
public class User
{
    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = "Inget namn";

    /// <summary>
    /// Gets or sets the hashed and salted version of the inserted password.
    /// </summary>
    public string Password { get; set; } = "Inget lösenord";

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; } = "Ingen e-mail";

    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public string Company { get; set; } = "Inget företagsnamn";

    /// <summary>
    /// Gets the date when the user was created.
    /// </summary>
    public DateTime DateCreated { get; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the date of the latest payment by the user. It is set to 1/1/0001 12:00:00 AM by default.
    /// </summary>
    public DateTime LatestPaymentDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets favorite contracts.
    /// </summary>
    public ICollection<Contract> Favorites { get; init; } = new List<Contract>();

    /// <summary>
    /// Gets the most recently viewed contracts.
    /// </summary>
    public IList<RecentlyViewedContract> RecentlyViewContracts { get; } = new List<RecentlyViewedContract>();

    /// <summary>
    /// Creates a shallow copy.
    /// </summary>
    /// <returns>A shallow copy.</returns>
    public User Copy()
    {
        return new User
        {
            Company = this.Company,
            Email = this.Email,
            Favorites = this.Favorites,
            Id = this.Id,
            LatestPaymentDate = this.LatestPaymentDate,
            Name = this.Name,
            Password = this.Password,
        };
    }
}
