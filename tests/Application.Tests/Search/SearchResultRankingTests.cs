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
        _cut.AddModule(new SimpleTextSearch(contract => contract.Name, 5d));
        _cut.AddModule(new SimpleTextSearch(contract => contract.Description, 1d));
    }

    [Fact]
    public void Searching_SortsContractWithMatchingNameFirst_WhenThereIsAnotherOneWithMatchingDescription()
    {
        // Arrange
        const string content = "Contract content here";
        var nameContract = new Contract { Name = content, };
        var contracts = new Contract[] { new() { Description = content, }, nameContract, };

        // Act
        IEnumerable<Contract> results = _cut.Search(content, contracts).ToList();

        // Assert
        results.First().Should().BeEquivalentTo(nameContract);
    }

    // TODO: matches from a single module should be ordered internally, and the module itself should have an order in
    // TODO: the greater scheme of things (against other modules)

    // TODO: sort in descending order of close-to-query

    // TODO: use a scoring system that accumulates from different search modules for a single contract

    // TODO: use comparers with a constructor taking in the query
}
