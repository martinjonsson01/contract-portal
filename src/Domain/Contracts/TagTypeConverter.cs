using System.ComponentModel;
using System.Globalization;

namespace Domain.Contracts;

/// <summary>
/// Converts <see cref="Tag"/> instances to and from <see cref="string"/>s.
/// </summary>
public class TagTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value is not string textValue
            ? base.ConvertFrom(context, culture, value)
            : new Tag { Text = textValue, };
    }
}
