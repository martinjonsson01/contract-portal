namespace Domain.Contracts;

/// <summary>
/// Additional information that supplements a <see cref="Contract"/>.
/// </summary>
public class Document
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the identifier of the <see cref="Contract"/> this document belongs to.
    /// </summary>
    public Guid ContractId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets where to find the document.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
