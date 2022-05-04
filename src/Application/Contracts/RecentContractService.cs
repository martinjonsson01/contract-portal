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
    public RecentContractService()
    {
        _recent = new Collection<Contract>();
    }

    /// <inheritdoc />
    public int Size(string id)
    {
        return _recent.Count;
    }

    /// <param name="id"></param>
    /// <inheritdoc />
    public IEnumerable<Contract> FetchRecentContracts(string id)
    {
        return _recent;
    }

    /// <inheritdoc />
    public void Add(string id, Contract contract)
    {
        if (_recent.Any(recentContract => recentContract.Id.Equals(contract.Id)))
        {
            return;
        }

        const int recentAmountMax = 3;
        if (_recent.Count >= recentAmountMax)
        {
            RemoveLast(id);
        }

        _recent.Add(contract);
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        foreach (Contract contract in _recent)
        {
            if (contract.Id != id)
                continue;
            _ = _recent.Remove(contract);
            break;
        }
    }

    /// <inheritdoc />
    public void RemoveLast(string id)
    {
        _recent.RemoveAt(0);
    }
}
