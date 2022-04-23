using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
/// Searches tags in a <see cref="Contract"/>.
/// </summary>
public class TagSearch : ISearchModule<Contract>
{
    /// <summary>
    /// Matches the given query against the tags in a contract.
    /// </summary>
    /// <param name="entity">The contract.</param>
    /// <param name="query">The text containing the tags to match against.</param>
    /// <returns>Whether the contract contains any matching tags.</returns>
    public bool Match(Contract entity, string query)
    {
        string[] queryTags = query.Split(' ');
        return queryTags.Any(queryTag => entity.Tags.Contains(queryTag, StringComparer.OrdinalIgnoreCase));
    }
}
