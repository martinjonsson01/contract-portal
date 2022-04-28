namespace Domain.Contracts;

/// <summary>
/// A tag that can be used to add metadata to a <see cref="Contract"/>.
/// </summary>
public class Tag
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the identifier of the <see cref="Contract"/> which this tag belongs to.
    /// </summary>
    public Guid ContractId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the text content.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Converts to a string representation.
    /// </summary>
    /// <returns>A string representation.</returns>
    public override string ToString()
    {
        return Text;
    }
}
