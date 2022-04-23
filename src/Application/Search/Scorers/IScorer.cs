namespace Application.Search.Scorers;

/// <summary>
/// A way of scoring how well something matches a given string.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to assign a score to.</typeparam>
public interface IScorer<in TEntity>
{
    /// <summary>
    /// Scores the given entity based on the given query.
    /// </summary>
    /// <param name="entity">The entity to score.</param>
    /// <param name="query">The text to score the entity against.</param>
    /// <returns>
    /// A number in the range [0, 1] representing how well the entity matches the query string,
    /// where a value of 0 means that it does not match at all and 1 means it matches exactly.
    /// </returns>
    double Score(TEntity entity, string query);
}
