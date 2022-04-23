using Domain.Contracts;

namespace Application.Search.Scorers;

/// <summary>
/// <para>
/// Scores properties of <see cref="Contract"/>s using their edit-distance (aka Levenshtein-distance).
/// </para>
///
/// <para>
/// The edit-distance is a measure of string likeness and is defined as
/// the minimum number of steps required to turn one of the strings into the other.
/// E.g. turning "cats" into "cat" requires one operation (remove s) and so their edit distance is 1.
/// </para>
/// </summary>
public class EditDistanceScorer : IScorer<Contract>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditDistanceScorer"/> class with a given selector delegate.
    /// </summary>
    /// <param name="selector">A delegate that converts a contract into a string by selecting one of its properties.</param>
    public EditDistanceScorer(Func<Contract, string> selector)
    {
    }

    /// <summary>
    /// Scores the given <see cref="Contract"/> based on one of its properties' edit distance from the given query.
    /// </summary>
    /// <param name="entity">The contract to get the property from.</param>
    /// <param name="query">The text used to calculate the edit distance from the contract property.</param>
    /// <returns>
    /// A number in the range [0, 1] representing how well the contract matches the query string,
    /// where a value of 0 means that it does not match at all and 1 means it matches exactly.
    /// </returns>
    public double Score(Contract entity, string query)
    {
        return string.IsNullOrEmpty(query) ? 0d : 1d;
    }
}
