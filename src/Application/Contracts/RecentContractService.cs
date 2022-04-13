using System.Collections.ObjectModel;

using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class RecentContractService : IRecentContractService
{
    private readonly Collection<Contract>? _recent;

    /// <summary>
    /// Constructs recent contract service.
    /// </summary>
    /// <param name="recent">The list of recent contracts.</param>
    public RecentContractService(Collection<Contract> recent)
    {
        _recent = recent;
    }

    /// <inheritdoc />
    public void FilterRecentContracts(Contract contract)
    {
        if (_recent != null && _recent.Any(recentContract => recentContract.Id.Equals(contract.Id)))
        {
            return;
        }

        const int recentAmountMax = 3;
        if (_recent != null && _recent.Count >= recentAmountMax)
        {
            _recent.RemoveAt(0);
        }

        _recent?.Add(contract);
    }
}
