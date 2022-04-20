using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
/// Searches names of contracts.
/// </summary>
public class NameSearch : ISearchModule<Contract>
{
    /// <summary>
    /// Matches a <see cref="Contract"/> name to the given query.
    /// </summary>
    /// <param name="entity">The contract.</param>
    /// <param name="query">The text to match with.</param>
    /// <returns>Whether the contract matches the name in the query or not.</returns>
    public bool Match(Contract entity, string query)
    {
        return entity.Name.Contains(query, StringComparison.OrdinalIgnoreCase);
    }
}
