using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
///     Searches for substrings of the given query in contract properties.
/// </summary>
public class BodyTextSearch : ISearchModule<Contract>
{
    private readonly Func<Contract, string> _selector;

    /// <summary>
    /// Constructs a <see cref="BodyTextSearch"/> module for a given property of a <see cref="Contract"/>.
    /// </summary>
    /// <param name="selector">A delegate that returns the contents of one of the properties of a <see cref="Contract"/>.</param>
    public BodyTextSearch(Func<Contract, string> selector)
    {
        _selector = selector;
    }

    /// <summary>
    /// Matches a <see cref="Contract"/> property to the given query.
    /// </summary>
    /// <param name="entity">The contract.</param>
    /// <param name="query">The text to match with.</param>
    /// <returns>Whether the contract property matches the substrings of the query or not.</returns>
    public bool Match(Contract entity, string query)
    {
        string text = _selector(entity);

        if (string.IsNullOrEmpty(query))
            return false;

        string[] substrings = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return substrings.Any(predicate: s => text.Contains(s, StringComparison.OrdinalIgnoreCase));
    }
}
