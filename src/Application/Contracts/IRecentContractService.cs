using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Methods for recent contracts.
/// </summary>
public interface IRecentContractService
{
    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <returns>Top most recently viewed contracts.</returns>
    public IEnumerable<Contract> FetchRecentContracts();

    /// <summary>
    /// Ensures that a new contract is qualified as recently viewed.
    /// </summary>
    /// <param name="contract">Possible new recent contract.</param>
    public void AddRecent(Contract contract);
}
