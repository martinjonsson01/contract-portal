using Application.Exceptions;
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
        return _repo.All;
    }

    /// <inheritdoc />
    public void Add(Contract contract)
    {
        if (_repo.All.Any(otherContract => contract.Id.Equals(otherContract.Id)))
            throw new IdentifierAlreadyTakenException();

        _repo.Add(contract);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _repo.Remove(id);
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchFavorites()
    {
        return _repo.Favorites();
    }

    /// <inheritdoc />
    public void UpdateContract(Contract contract)
    {
        _repo.UpdateContract(contract);
    }

    /// <inheritdoc />
    public Contract FetchContract(Guid id)
    {
        return _repo.FetchContract(id) ?? throw new ContractDoesNotExistException();
    }
}
