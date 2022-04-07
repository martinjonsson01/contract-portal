﻿namespace Domain.Contracts;

/// <summary>
///     A document containing negotiated discounts and benefits.
/// </summary>
public class Contract
{
    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    ///     Gets or sets the name of the contract supplier.
    /// </summary>
    public string Name { get; set; } = "No name";

    /// <summary>
    ///     Gets or sets the description.
    /// </summary>
    public string Description { get; set; } = "No description.";

    /// <summary>
    ///     Gets or sets image to the supplier logo.
    /// </summary>
    public string ImagePath { get; set; } = "images/sj.png";
}
