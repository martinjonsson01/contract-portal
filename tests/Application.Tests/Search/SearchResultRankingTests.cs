using System.Linq;

using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Tests.Search;

public class SearchResultRankingTests
{
    private readonly SearchEngine<Contract> _cut;

    public SearchResultRankingTests()
    {
        _cut = new SearchEngine<Contract>();
    }

    [Fact]
    public void Searching_SortsContractWithMatchingNameFirst_WhenThereIsAnotherOneWithMatchingDescription()
    {
        // Arrange
        _cut.AddModule(new SimpleTextSearch(contract => contract.Name, 5d));
        _cut.AddModule(new SimpleTextSearch(contract => contract.Description, 1d));

        const string content = "Contract content here";
        var nameContract = new Contract { Name = content, };
        var contracts = new Contract[] { new() { Description = content, }, nameContract, };

        // Act
        IEnumerable<Contract> results = _cut.Search(content, contracts).ToList();

        // Assert
        results.First().Should().BeEquivalentTo(nameContract);
    }

    [Fact]
    public void Searching_SortsContractWithMultipleLowerMatchesFirst_WhenThereIsAnotherWithASingleHighWeightMatch()
    {
        // Arrange
        _cut.AddModule(new SimpleTextSearch(contract => contract.Name, 5d));
        _cut.AddModule(new SimpleTextSearch(contract => contract.Description, 2d));
        _cut.AddModule(new SimpleTextSearch(contract => contract.Instructions, 2d));
        _cut.AddModule(new SimpleTextSearch(contract => contract.SupplierName, 2d));

        const string content = "Contract content here";
        var contractWithSingleHighValueMatch = new Contract { Name = content, };
        var contractWithMultipleLowValueMatches = new Contract
        {
            Description = content,
            Instructions = content,
            SupplierName = content,
        };
        var contracts = new Contract[] { contractWithSingleHighValueMatch, contractWithMultipleLowValueMatches, };

        // Act
        IEnumerable<Contract> results = _cut.Search(content, contracts).ToList();

        // Assert
        results.First().Should().BeEquivalentTo(contractWithMultipleLowValueMatches);
    }
}
