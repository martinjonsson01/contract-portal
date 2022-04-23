using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search.Modules;

public class TagSearchModuleTests
{
    private readonly ISearchModule<Contract> _cut;

    public TagSearchModuleTests()
    {
        _cut = new TagSearch();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Match_ReturnsTrue_WhenQueryIsOneOfTheTags(int tagIndex)
    {
        // Arrange
        string[] tags = { "Tag1", "Tag2", "Tag3", };
        var contract = new Contract { Tags = tags, };

        // Act
        bool matches = _cut.Match(contract, tags[tagIndex]);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsNoneOfTheTags()
    {
        // Arrange
        string[] tags = { "Tag1", "Tag2", "Tag3", };
        var contract = new Contract { Tags = tags, };

        // Act
        bool matches = _cut.Match(contract, "other-tag");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsPartialTag()
    {
        // Arrange
        string[] tags = { "Tag1", "Tag2", "Tag3", };
        var contract = new Contract { Tags = tags, };

        // Act
        bool matches = _cut.Match(contract, tags[0].Substring(2));

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsAllTags()
    {
        // Arrange
        string[] tags = { "Tag1", "Tag2", "Tag3", };
        var contract = new Contract { Tags = tags, };

        // Act
        bool matches = _cut.Match(contract, string.Join(' ', tags));

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsInOtherCaseThanTag()
    {
        // Arrange
        string[] tags = { "Tag1", "Tag2", "Tag3", };
        var contract = new Contract { Tags = tags, };

        // Act
        bool matches = _cut.Match(contract, tags[0].ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }
}
