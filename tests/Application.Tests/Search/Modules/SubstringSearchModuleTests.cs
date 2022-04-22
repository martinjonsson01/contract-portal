using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search.Modules;

public class SubstringSearchModuleTests
{
    private readonly ISearchModule<Contract> _cutDescription;
    private readonly ISearchModule<Contract> _cutName;

    public SubstringSearchModuleTests()
    {
        _cutDescription = new SubstringSearch(contract => contract.Description);
        _cutName = new SubstringSearch(contract => contract.Name);
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsSameAsDescription()
    {
        // Arrange
        const string description = "A short description";
        var contract = new Contract { Description = description, };

        // Act
        bool matches = _cutDescription.Match(contract, description);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsSubstringOfDescription()
    {
        // Arrange
        var contract = new Contract { Description = "A short description", };

        // Act
        bool matches = _cutDescription.Match(contract, "short");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsMultipleSubstringsOfDescription()
    {
        // Arrange
        const string description = "A longer description to match against";
        var contract = new Contract { Description = description, };

        // Act
        bool matches = _cutDescription.Match(contract, "against longer to");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsEntirelyDifferentFromDescription()
    {
        // Arrange
        var contract = new Contract { Description = "A longer description to match against", };

        // Act
        bool matches = _cutDescription.Match(contract, "We build bridges");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsEmpty()
    {
        // Arrange
        var contract = new Contract { Description = "A short description", };

        // Act
        bool matches = _cutDescription.Match(contract, string.Empty);

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryDoesNotContainPartOfDescription()
    {
        // Arrange
        var contract = new Contract { Description = "A short description", };

        // Act
        bool matches = _cutDescription.Match(contract, "non matching text");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsHalfOfName()
    {
        // Arrange
        const string name = "A short description";
        var contract = new Contract { Description = name, };

        // Act
        bool matches = _cutDescription.Match(contract, name.Substring(name.Length / 2));

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsInUpperCase()
    {
        // Arrange
        const string description = "A short description";
        var contract = new Contract { Description = description, };

        // Act
        bool matches = _cutDescription.Match(contract, description.ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsMatchingSubstring()
    {
        // Arrange
        const string description = "A short description";
        var contract = new Contract { Description = description, };

        // Act
        bool matches = _cutDescription.Match(contract, "des");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsMatchingName()
    {
        // Arrange
        const string name = "Saab";
        var contract = new Contract { Name = name, };

        // Act
        bool matches = _cutName.Match(contract, name);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsPartOfMultipartName()
    {
        // Arrange
        var contract = new Contract { Name = "The great iron company", };

        // Act
        bool matches = _cutName.Match(contract, "great company");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryIsSpaces()
    {
        // Arrange
        var contract = new Contract { Name = "The great iron company", };

        // Act
        bool matches = _cutName.Match(contract, "    ");

        // Assert
        matches.Should().BeFalse();
    }
}
