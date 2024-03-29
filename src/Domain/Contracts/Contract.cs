﻿using Domain.Users;

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
    ///     Gets or sets the name of the contract.
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
    ///     Gets or sets the FAQ.
    /// </summary>
    public string FAQ { get; set; } = string.Empty;

    /// <summary>
    ///     Gets the search keywords that have been tagged.
    /// </summary>
    public IList<Tag> Tags { get; init; } = new List<Tag>();

    /// <summary>
    ///     Gets or sets image to the supplier logo.
    /// </summary>
    public string SupplierLogoImagePath { get; set; } = "images/sj.png";

    /// <summary>
    ///     Gets or sets an inspirational image.
    /// </summary>
    public string InspirationalImagePath { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets an additional document from the supplier.
    /// </summary>
    public Document? AdditionalDocument { get; set; }

    /// <summary>
    ///     Gets or sets the supplier name.
    /// </summary>
    public string SupplierName { get; set; } = "Ingen leverantör angiven.";

    /// <summary>
    ///     Gets or sets the description of the supplier.
    /// </summary>
    public string SupplierDescription { get; set; } = "Ingen leverantörbeskrivning angiven.";

    /// <summary>
    ///     Gets or sets the contact information for the supplier.
    /// </summary>
    public string SupplierContactInfo { get; set; } = "Kontaktinformation till leverantör saknas.";

    // Entity Framework requires that a navigation property exists in both
    // classes (User and Contract) when a many-to-many relation is to be genereated,
    // and exists only for this reason.
    // See the Entity Framework documentation for more information.
    private ICollection<User> FavoritedBy { get; } = new List<User>();
}
