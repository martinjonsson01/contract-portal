using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search.Modules;

public class TagSearchModuleTests
{
    private readonly ISearchModule<Contract> _cut;
    private readonly Contract _contract;

    public TagSearchModuleTests()
    {
        _cut = new TagSearch();

        string[] tags = { "Tag1", "Tag2", "Tag3", };
        _contract = new Contract { Tags = tags, };
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Match_ReturnsTrue_WhenQueryIsOneOfTheTags(int tagIndex)
    {
        // Arrange

        // Act
        bool matches = _cut.Match(_contract, _contract.Tags[tagIndex]);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsNoneOfTheTags()
    {
        // Arrange

        // Act
        bool matches = _cut.Match(_contract, "other-tag");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsPartialTag()
    {
        // Arrange

        // Act
        bool matches = _cut.Match(_contract, _contract.Tags[0].Substring(2));

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsAllTags()
    {
        // Arrange

        // Act
        bool matches = _cut.Match(_contract, string.Join(' ', _contract.Tags));

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsInOtherCaseThanTag()
    {
        // Arrange

        // Act
        bool matches = _cut.Match(_contract, _contract.Tags[0].ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsEmpty()
    {
        // Arrange

        // Act
        bool matches = _cut.Match(_contract, string.Empty);

        // Assert
        matches.Should().BeFalse();
    }
}
