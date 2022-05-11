using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class RecentContractService : IRecentContractService
{
    private readonly IRecentContractRepository _recent;
    private readonly IContractRepository _contract;

    /// <summary>
    /// Constructs recent contract service.
    /// </summary>
    /// <param name="recent">The recent repository to fetch and update data from and to.</param>
    /// <param name="contract">The contract repository to fetch the contracts from.</param>
    public RecentContractService(IRecentContractRepository recent, IContractRepository contract)
    {
        _recent = recent;
        _contract = contract;
    }

    /// <inheritdoc />
    public int Size(string id)
    {
        return _recent.FetchRecentContracts(id).Count;
    }

    /// <param name="id"></param>
    /// <inheritdoc />
    public IEnumerable<Contract> FetchRecentContracts(string id)
    {
        var contracts = new List<Contract>();
        foreach (RecentlyViewedContract recentContract in _recent.FetchRecentContracts(id)
                     .OrderByDescending(recentContract => recentContract.LastViewed))
        {
            contracts.Add(_contract.FetchContract(recentContract.ContractId) ??
                          throw new ContractDoesNotExistException());
        }

        return contracts;
    }

    /// <inheritdoc />
    public void Add(string id, Contract contract)
    {
        _recent.AddRecent(id, contract);

        const int recentAmountMax = 3;
        if (Size(id) <= recentAmountMax)
            return;

        RecentlyViewedContract toRemove = _recent
            .FetchRecentContracts(id)
            .OrderBy(recentContract => recentContract.LastViewed)
            .First();
        _recent.RemoveRecent(toRemove);
    }
}
