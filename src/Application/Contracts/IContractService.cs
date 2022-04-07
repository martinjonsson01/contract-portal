using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Used for interacting with contract logic.
/// </summary>
public interface IContractService
{
    /// <summary>
    /// Gets all contracts.
    /// </summary>
    /// <returns>All contracts.</returns>
    IEnumerable<Contract> FetchAllContracts();

    /// <summary>
    /// Adds a new contract.
    /// </summary>
    /// <param name="contract">The new contract.</param>
    void Add(Contract contract);
}
