using Application.Users;
using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class LimitedRecentContractService : IRecentContractService
{
    private readonly IRecentContractRepository _recent;
    private readonly IContractRepository _contract;

    /// <summary>
    /// Constructs recent contract service.
    /// </summary>
    /// <param name="recent">The recent repository to fetch and update data from and to.</param>
    /// <param name="contract">The contract repository to fetch the contracts from.</param>
    public LimitedRecentContractService(IRecentContractRepository recent, IContractRepository contract)
    {
        _recent = recent;
        _contract = contract;
    }

    /// <inheritdoc />
    public int Size(Guid userId)
    {
        return _recent.FetchRecentContracts(userId).Count;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchRecentContracts(Guid userId)
    {
        try
        {
            var contracts = new List<Contract>();
            foreach (RecentlyViewedContract recentContract in _recent.FetchRecentContracts(userId)
                         .OrderByDescending(recentContract => recentContract.LastViewed))
            {
                contracts.Add(_contract.FetchContract(recentContract.ContractId) ??
                              throw new ContractDoesNotExistException());
            }

            return contracts;
        }
        catch (UserDoesNotExistException)
        {
            return new List<Contract>();
        }
    }

    /// <inheritdoc />
    public void Add(Guid userId, Contract contract)
    {
        _recent.Add(userId, contract);

        const int recentAmountMax = 3;
        if (Size(userId) <= recentAmountMax)
            return;

        RecentlyViewedContract toRemove = _recent
            .FetchRecentContracts(userId)
            .OrderBy(recentContract => recentContract.LastViewed)
            .First();
        _recent.Remove(toRemove);
    }
}
