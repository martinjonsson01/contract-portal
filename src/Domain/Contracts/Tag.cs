using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Domain.Contracts;

/// <summary>
/// A tag that can be used to add metadata to an object.
/// </summary>
[TypeConverter(typeof(TagTypeConverter))]
[JsonConverter(typeof(TagJsonConverter))]
public class Tag
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

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
