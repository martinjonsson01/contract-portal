using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search.Modules;

public class ContractSupplierNameSearchModuleTests
{
    private readonly ISearchModule<Contract> _cut;

    public ContractSupplierNameSearchModuleTests()
    {
        _cut = new SupplierNameSearch();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsSupplierName()
    {
        // Arrange
        const string supplierName = "Supplier name";
        var contract = new Contract { SupplierName = supplierName, };

        // Act
        bool matches = _cut.Match(contract, supplierName);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsEmpty()
    {
        // Arrange
        var contract = new Contract { SupplierName = "Supplier name", };

        // Act
        bool matches = _cut.Match(contract, string.Empty);

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQueryDoesNotContainSupplierName()
    {
        // Arrange
        var contract = new Contract { SupplierName = "Supplier name", };

        // Act
        bool matches = _cut.Match(contract, "Not the distributor");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryIsHalfOfSupplierName()
    {
        // Arrange
        const string supplierName = "Supplier name";
        var contract = new Contract { SupplierName = supplierName, };

        // Act
        bool matches = _cut.Match(contract, supplierName.Substring(supplierName.Length / 2));

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQuerySupplierNameInUpperCase()
    {
        // Arrange
        const string supplierName = "Supplier name";
        var contract = new Contract { SupplierName = supplierName, };

        // Act
        bool matches = _cut.Match(contract, supplierName.ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsFalse_WhenQuerySupplierNameInDifferentOrder()
    {
        // Arrange
        var contract = new Contract { SupplierName = "Supplier name", };

        // Act
        bool matches = _cut.Match(contract, "name Supplier");

        // Assert
        matches.Should().BeFalse();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsSupplierNameAndMore()
    {
        // Arrange
        const string supplierName = "Supplier name";
        var contract = new Contract { SupplierName = supplierName, };

        // Act
        bool matches = _cut.Match(contract, supplierName + " more");

        // Assert
        matches.Should().BeTrue();
    }

    [Fact]
    public void Match_ReturnsTrue_WhenQueryContainsSupplierNameAndMoreInUpperCase()
    {
        // Arrange
        const string supplierName = "Supplier name";
        var contract = new Contract { SupplierName = supplierName, };

        // Act
        bool matches = _cut.Match(contract, (supplierName + "more").ToUpperInvariant());

        // Assert
        matches.Should().BeTrue();
    }
}
