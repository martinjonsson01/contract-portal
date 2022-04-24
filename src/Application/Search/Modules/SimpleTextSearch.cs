using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
/// Searches text in a given field of a contract.
/// </summary>
public class SimpleTextSearch : ISearchModule<Contract>
{
    /// <summary>
    /// Constructs a <see cref="SimpleTextSearch"/> module for a given property of a <see cref="Contract"/>.
    /// </summary>
    /// <param name="selector">A delegate that returns the contents of one of the properties of a <see cref="Contract"/>.</param>
    /// <param name="weight">How the matches of this search module should be weighted relative to other modules.</param>
    public SimpleTextSearch(Func<Contract, string> selector, double weight = 1d)
    {
        Weight = weight;
        Selector = selector;
    }

    /// <inheritdoc />
    public double Weight { get; }

    private Func<Contract, string> Selector { get; }

    /// <summary>
    /// Matches a <see cref="Contract"/> property to the given query.
    /// </summary>
    /// <param name="entity">The contract.</param>
    /// <param name="query">The text to match with.</param>
    /// <returns>Whether the contract matches the name in the query or not.</returns>
    public bool Match(Contract entity, string query)
    {
        if (string.IsNullOrEmpty(query))
            return false;

        string text = Selector(entity);
        return text.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               query.Contains(text, StringComparison.OrdinalIgnoreCase);
    }
}
