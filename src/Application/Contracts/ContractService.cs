using Application.Exceptions;
using Application.Search;
using Application.Search.Modules;

using Domain.Contracts;

namespace Application.Contracts;

/// <inheritdoc />
public class ContractService : IContractService
{
    private readonly IContractRepository _repo;
    private readonly SearchEngine<Contract> _search;

    /// <summary>
    /// Constructs contract service.
    /// </summary>
    /// <param name="repo">Where to store and fetch contracts from.</param>
    /// <param name="search">The search engine to use when searching through contracts.</param>
    public ContractService(IContractRepository repo, SearchEngine<Contract> search)
    {
        _repo = repo;
        _search = search;
        _search.AddModule(new NameSearch());
        _search.AddModule(new SubstringSearch(contract => contract.Description));
        _search.AddModule(new SubstringSearch(contract => contract.SupplierDescription));
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchAllContracts()
    {
        return _repo.All;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchRecentContracts()
    {
        return _repo.Recent;
    }

    /// <inheritdoc />
    public void Add(Contract contract)
    {
        if (_repo.All.Any(otherContract => contract.Id.Equals(otherContract.Id)))
            throw new IdentifierAlreadyTakenException();

        _repo.Add(contract);
    }

    /// <inheritdoc />
    public void AddRecent(Contract contract)
    {
        _repo.AddRecent(contract);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _repo.Remove(id);
    }

    /// <inheritdoc />
    public IEnumerable<Contract> Search(string query)
    {
        return _search.Search(query, FetchAllContracts());
    }

    /// <inheritdoc />
    public IEnumerable<Contract> FetchFavorites()
    {
        return _repo.Favorites;
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
