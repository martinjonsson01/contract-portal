using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// A data transfer object for a preview of a <see cref="Contract"/>.
/// </summary>
public class ContractPreviewDto
{
    /// <summary>
    /// Gets the unique identifier of the <see cref="Contract"/> this is a preview of.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    ///     Gets or sets the name of the contract.
    /// </summary>
    public string Name { get; set; } = "Inget namn";

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    public string Description { get; set; } = "Det finns ingen beskrivning.";

    /// <summary>
    ///     Gets or sets image to the supplier logo.
    /// </summary>
    public string SupplierLogoImagePath { get; set; } = "images/sj.png";

    /// <summary>
    ///     Gets or sets an inspirational image.
    /// </summary>
    public string InspirationalImagePath { get; set; } = string.Empty;
}
