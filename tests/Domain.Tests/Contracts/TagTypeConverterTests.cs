using System;

using Domain.Contracts;

using FluentAssertions.Execution;

namespace Domain.Tests.Contracts;

public class TagTypeConverterTests
{
    private readonly TagTypeConverter _cut;

    public TagTypeConverterTests()
    {
        _cut = new TagTypeConverter();
    }

    [Fact]
    public void Should_Fail()
    {
        false.Should().BeTrue();
    }

    [Fact]
    public void Converter_CanConvertFromString()
    {
        // Arrange

        // Act
        bool canConvert = _cut.CanConvertFrom(null, typeof(string));

        // Assert
        canConvert.Should().BeTrue();
    }

    [Fact]
    public void Converter_CanNotConvertFromBool()
    {
        // Arrange

        // Act
        bool canConvert = _cut.CanConvertFrom(null, typeof(bool));

        // Assert
        canConvert.Should().BeFalse();
    }

    [Fact]
    public void ConvertFromString_CreatesTagWithCorrectText()
    {
        // Arrange
        var expected = new Tag { Text = "Tag text", };

        // Act
        var actual = (Tag?)_cut.ConvertFrom(null, null, expected.Text);

        // Assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual!.Text.Should().Be(expected.Text);
        }
    }

    [Fact]
    public void ConvertFromString_DoesNotCreateTag_WhenValueIsNotString()
    {
        // Arrange

        // Act
        Action convert = () => _cut.ConvertFrom(null, null, 123);

        // Assert
        convert.Should().Throw<NotSupportedException>();
    }
}
