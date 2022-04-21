using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
/// Searches text in a given field of a contract.
/// </summary>
public class SimpleTextSearch : ISearchModule<Contract>
{
    /// <summary>
    /// Matches a <see cref="Contract"/> property to the given query.
    /// </summary>
    /// <param name="entity">The contract.</param>
    /// <param name="query">The text to match with.</param>
    /// <returns>Whether the contract matches the name in the query or not.</returns>
    public bool Match(Contract entity, string query)
    {
        return entity.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               query.Contains(entity.Name, StringComparison.OrdinalIgnoreCase);
    }
}
