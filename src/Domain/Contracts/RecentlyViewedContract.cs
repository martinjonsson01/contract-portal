using Domain.Users;

namespace Domain.Contracts;

/// <summary>
///     A container for a contract and when it was viewed.
/// </summary>
public class RecentlyViewedContract
{
    /// <summary>
    /// Creates a instance of a recently viewed contract with the current time and contract id.
    /// </summary>
    /// <param name="contractId">The viewed contract's id.</param>
    /// <param name="userId">The viewer's id.</param>
    public RecentlyViewedContract(Guid contractId, Guid userId)
    {
        UserId = userId;
        ContractId = contractId;
        LastViewed = DateTime.Now;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RecentlyViewedContract"/> class.
    /// </summary>
    public RecentlyViewedContract()
        : this(default, default)
    {
    }

    /// <summary>
    /// Gets or sets the time for when the contract was viewed.
    /// </summary>
    public DateTime LastViewed { get; set; }

    /// <summary>
    /// Gets the viewed contract's id.
    /// </summary>
    public Guid ContractId { get; }

    /// <summary>
    /// Gets the viewers id.
    /// </summary>
    public Guid UserId { get; }
}
