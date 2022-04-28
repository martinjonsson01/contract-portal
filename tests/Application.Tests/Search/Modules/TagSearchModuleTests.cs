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

        Tag[] tags = { new() { Text = "Tag1", }, new() { Text = "Tag2", }, new() { Text = "Tag3", }, };
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
        bool matches = _cut.Match(_contract, _contract.Tags[tagIndex].ToString());

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
        bool matches = _cut.Match(_contract, _contract.Tags[0].ToString().Substring(2));

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
        bool matches = _cut.Match(_contract, _contract.Tags[0].ToString().ToUpperInvariant());

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
