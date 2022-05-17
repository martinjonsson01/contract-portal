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
    public int Size(string username)
    {
        return _recent.FetchRecentContracts(username).Count;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchRecentContracts(string username)
    {
        try
        {
            var contracts = new List<Contract>();
            foreach (RecentlyViewedContract recentContract in _recent.FetchRecentContracts(username)
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
    public void Add(string username, Contract contract)
    {
        _recent.Add(username, contract);

        const int recentAmountMax = 3;
        if (Size(username) <= recentAmountMax)
            return;

        RecentlyViewedContract toRemove = _recent
            .FetchRecentContracts(username)
            .OrderBy(recentContract => recentContract.LastViewed)
            .First();
        _recent.Remove(toRemove);
    }
}
