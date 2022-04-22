using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search.Modules;

public class BodyTextSearchModuleTests
{
    private readonly ISearchModule<Contract> _cut;

    public BodyTextSearchModuleTests()
    {
        _cut = new BodyTextSearch(contract => contract.Description);
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsSameAsDescription()
    {
        // Arrange
        const string description = "A short description";
        var contract = new Contract { Description = description, };

        // Act
        bool matches = _cut.Match(contract, description);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsPartOfDescription()
    {
        // Arrange
        var contract = new Contract { Description = "A short description", };

        // Act
        bool matches = _cut.Match(contract, "short");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsMultiplePartsOfDescription()
    {
        // Arrange
        var contract = new Contract { Description = "A longer description to match against", };

        // Act
        bool matches = _cut.Match(contract, "against to longer");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsEntirelyDifferentFromDescription()
    {
        // Arrange
        var contract = new Contract { Description = "A longer description to match against", };

        // Act
        bool matches = _cut.Match(contract, "We build bridges");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsEmpty()
    {
        // Arrange
        var contract = new Contract { Description = "A short description", };

        // Act
        bool matches = _cut.Match(contract, string.Empty);

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsInUpperCase()
    {
        // Arrange
        const string description = "A short description";
        var contract = new Contract { Description = description, };

        // Act
        bool matches = _cut.Match(contract, description.ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsSpaces()
    {
        // Arrange
        var contract = new Contract { Description = "A short description", };

        // Act
        bool matches = _cut.Match(contract, "    ");

        // Assert
        matches.Should().BeFalse();
    }
}
