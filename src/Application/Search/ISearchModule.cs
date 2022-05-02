namespace Application.Search;

/// <summary>
/// A plug-in to a <see cref="SearchEngine{TEntity}"/> that matches entities to queries based on certain criteria.
///
/// E.g. an implementation of this interface could match all integer-entities
/// that are divisible by the number in the query.
/// </summary>
/// <typeparam name="TEntity">The entity to work on.</typeparam>
public interface ISearchModule<in TEntity>
{
    /// <summary>
    /// Gets how heavily the matches of this search module should be weighted relative to other modules.
    /// </summary>
    double Weight { get; }

    /// <summary>
    /// Matches an entity to a query and returns whether it is a successful match or not.
    /// </summary>
    /// <param name="entity">The entity to analyse.</param>
    /// <param name="query">The text to match with.</param>
    /// <returns>Whether the entity matches the query or not.</returns>
    bool Match(TEntity entity, string query);
}
