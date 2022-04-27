using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Contracts;

/// <summary>
/// Converts tags to and from JSON.
/// </summary>
public class TagJsonConverter : JsonConverter<Tag>
{
    /// <inheritdoc/>
    public override Tag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = Guid.Parse(reader.GetString() ?? string.Empty);
        string text = reader.GetString() ?? string.Empty;

        return new Tag { Id = id, Text = text, };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Tag value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Id);
        writer.WriteStringValue(value.Text);
    }
}
