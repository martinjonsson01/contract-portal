using System.Text.Json.Serialization;

using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// A data transfer object for a preview of a <see cref="Contract"/>.
/// </summary>
public class ContractPreviewDto
{
    /// <summary>
    /// Creates a preview of the given <see cref="Contract"/>.
    /// </summary>
    /// <param name="contract">The original to make a preview of.</param>
    public ContractPreviewDto(Contract contract)
    {
        Id = contract.Id;
        Name = contract.Name;
        Description = contract.Description;
        SupplierLogoImagePath = contract.SupplierLogoImagePath;
        InspirationalImagePath = contract.InspirationalImagePath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractPreviewDto"/> class.
    /// </summary>
    [JsonConstructor]
    public ContractPreviewDto()
    {
        Id = Guid.Empty;
        Name = "Uninitialized";
        Description = "Uninitialized";
        SupplierLogoImagePath = "Uninitialized";
        InspirationalImagePath = "Uninitialized";
    }

    /// <summary>
    /// Gets the unique identifier of the <see cref="Contract"/> this is a preview of.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets or sets the name of the contract.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     Gets or sets image to the supplier logo.
    /// </summary>
    public string SupplierLogoImagePath { get; set; }

    /// <summary>
    ///     Gets or sets an inspirational image.
    /// </summary>
    public string InspirationalImagePath { get; set; }
}
