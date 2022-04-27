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

    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(Tag) || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc />
    public override object? ConvertTo(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType)
    {
        return value is not string text
            ? base.ConvertTo(context, culture, value, destinationType)
            : new Tag { Text = text, };
    }
}
