using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Methods for recent contracts.
/// </summary>
public interface IRecentContractService
{
    /// <summary>
    /// Gets how many recent contracts there are for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The number of current recent contracts.</returns>
    int Size(Guid userId);

    /// <summary>
    /// Gets the most recent contracts that the user has viewed.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>Top most recently viewed contracts.</returns>
    IEnumerable<Contract> FetchRecentContracts(Guid userId);

    /// <summary>
    /// Ensures that a new contract is qualified as recently viewed.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="contract">Possible new recent contract.</param>
    void Add(Guid userId, Contract contract);
}
