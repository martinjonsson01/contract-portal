namespace Application.Search.Modules;

/// <summary>
/// Matches all contracts against an empty query.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to always match empty queries against.</typeparam>
public class EmptySearch<TEntity> : ISearchModule<TEntity>
{
    /// <summary>
    /// Matches an entity against a query only if it is empty.
    /// </summary>
    /// <param name="entity">The entity to match against.</param>
    /// <param name="query">The text input query.</param>
    /// <returns>Whether the query is empty or not.</returns>
    public bool Match(TEntity entity, string query)
    {
        return string.IsNullOrEmpty(query);
    }
}
