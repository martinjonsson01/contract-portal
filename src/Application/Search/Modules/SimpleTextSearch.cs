using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
/// Searches text in a given field of a contract.
/// </summary>
public class SimpleTextSearch : ISearchModule<Contract>
{
    private readonly Func<Contract, string> _selector;

    /// <summary>
    /// Constructs a <see cref="SimpleTextSearch"/> module for a given property of a <see cref="Contract"/>.
    /// </summary>
    /// <param name="selector">A delegate that returns the contents of one of the properties of a <see cref="Contract"/>.</param>
    public SimpleTextSearch(Func<Contract, string> selector)
    {
        _selector = selector;
    }

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

        string text = _selector(entity);
        return text.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               query.Contains(text, StringComparison.OrdinalIgnoreCase);
    }
}
