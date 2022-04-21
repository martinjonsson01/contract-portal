using Domain.Contracts;

namespace Application.Search.Modules;

/// <summary>
/// Searches supplier names of contracts.
/// </summary>
public class SupplierNameSearch : ISearchModule<Contract>
{
    /// <summary>
    /// Matches a <see cref="Contract"/> supplier name to the given query.
    /// </summary>
    /// <param name="entity">The contract.</param>
    /// <param name="query">The text to match with.</param>
    /// <returns>Whether the contract matches the supplier name in query or not.</returns>
    /// <exception cref="NotImplementedException">not empy.</exception>
    public bool Match(Contract entity, string query)
    {
        return entity.SupplierName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               query.Contains(entity.SupplierName, StringComparison.OrdinalIgnoreCase);
    }
}
