using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class RecentContractService : IRecentContractService
{
    private readonly IRecentContractRepository _recent;

    /// <summary>
    /// Constructs recent contract service.
    /// </summary>
    /// <param name="recent">The recent repository to fetch and update data from and to.</param>
    public RecentContractService(IRecentContractRepository recent)
    {
        _recent = recent;
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
        return _recent.FetchRecentContracts(id);
    }

    /// <inheritdoc />
    public void Add(string id, Contract contract)
    {
        const int recentAmountMax = 3;
        if (Size(id) >= recentAmountMax)
        {
            _recent.RemoveLast(id);
        }

        _recent.Add(id, contract);
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        _recent.RemoveContract(id);
    }
}
