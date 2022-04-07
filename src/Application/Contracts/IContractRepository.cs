using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Logic for fetching and storing contracts.
/// </summary>
public interface IContractRepository
{
    /// <summary>
    /// Gets all contracts.
    /// </summary>
    public IEnumerable<Contract> Contracts { get; }
}
