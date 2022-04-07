using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class ContractService : IContractService
{
    private readonly IContractRepository _repo;

    /// <summary>
    /// Constructs contract service.
    /// </summary>
    /// <param name="repo">Where to store and fetch contracts from.</param>
    public ContractService(IContractRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchAllContracts()
    {
        return _repo.Contracts;
    }
}
