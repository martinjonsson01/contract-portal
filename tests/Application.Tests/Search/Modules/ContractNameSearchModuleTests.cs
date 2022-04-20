using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search.Modules;

public class ContractNameSearchModuleTests
{
    private readonly ISearchModule<Contract> _cut;

    public ContractNameSearchModuleTests()
    {
        _cut = new NameSearch();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsName()
    {
        // Arrange
        const string name = "Contract name";
        var contract = new Contract { Name = name, };

        // Act
        bool matches = _cut.Match(contract, name);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsEmpty()
    {
        // Arrange
        var contract = new Contract { Name = "Contract name", };

        // Act
        bool matches = _cut.Match(contract, string.Empty);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryDoesNotContainName()
    {
        // Arrange
        var contract = new Contract { Name = "Contract name", };

        // Act
        bool matches = _cut.Match(contract, "Not the title");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsHalfOfName()
    {
        // Arrange
        const string name = "Contract name";
        var contract = new Contract { Name = name, };

        // Act
        bool matches = _cut.Match(contract, name.Substring(name.Length / 2));

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryNameInUpperCase()
    {
        // Arrange
        const string name = "Contract name";
        var contract = new Contract { Name = name, };

        // Act
        bool matches = _cut.Match(contract, name.ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryNameInDifferentOrder()
    {
        // Arrange
        var contract = new Contract { Name = "Contract name", };

        // Act
        bool matches = _cut.Match(contract, "name Contract");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsNameAndMore()
    {
        // Arrange
        const string name = "Contract name";
        var contract = new Contract { Name = name, };

        // Act
        bool matches = _cut.Match(contract, name + " more");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsNameAndMoreInUpperCase()
    {
        // Arrange
        const string name = "Contract name";
        var contract = new Contract { Name = name, };

        // Act
        bool matches = _cut.Match(contract, (name + "more").ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }
}
