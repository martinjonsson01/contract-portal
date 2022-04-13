using System.Collections.ObjectModel;

using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class RecentContractService : IRecentContractService
{
    private readonly Collection<Contract> _recent;

    /// <summary>
    /// Constructs recent contract service.
    /// </summary>
    /// <param name="recent">The list of recent contracts.</param>
    public RecentContractService(Collection<Contract> recent)
    {
        _recent = recent;
    }

    /// <inheritdoc />
    public int Size()
    {
        return _recent.Count;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchRecentContracts()
    {
        return _recent;
    }

    /// <inheritdoc />
    public void AddRecent(Contract contract)
    {
        if (_recent.Any(recentContract => recentContract.Id.Equals(contract.Id)))
        {
            return;
        }

        const int recentAmountMax = 3;
        if (_recent.Count >= recentAmountMax)
        {
            _recent.RemoveAt(0);
        }

        _recent.Add(contract);
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        foreach (Contract contract in _recent)
        {
            if (contract.Id == id)
            {
                _ = _recent.Remove(contract);
            }
        }
    }
}
