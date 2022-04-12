namespace Domain.Contracts;

/// <summary>
///     A document containing negotiated discounts and benefits.
/// </summary>
public class Contract
{
    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets a value indicating whether the contract is a favorite or not.
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    ///     Gets or sets the name of the contract supplier.
    /// </summary>
    public string Name { get; set; } = "Inget namn";

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    public string Description { get; set; } = "Det finns ingen beskrivning.";

    /// <summary>
    ///     Gets or sets the instructions on how to use the discount.
    /// </summary>
    public string Instructions { get; set; } = "Det finns inga instruktioner.";

    /// <summary>
    ///     Gets or sets image to the supplier logo.
    /// </summary>
    public string ImagePath { get; set; } = "images/sj.png";
}
