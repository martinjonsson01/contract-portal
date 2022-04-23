using Domain.Contracts;

namespace Application.Search.Scorers;

/// <summary>
/// Always scores everything with the multiplicative identity (1).
/// </summary>
public class IdentityScorer : IScorer<Contract>
{
    /// <summary>
    /// Scores the given <see cref="Contract"/> a value of 1.
    /// </summary>
    /// <param name="entity">The contract to score.</param>
    /// <param name="query">The query to score against.</param>
    /// <returns>1 regardless of the input.</returns>
    public double Score(Contract entity, string query)
    {
        return 1d;
    }
}
