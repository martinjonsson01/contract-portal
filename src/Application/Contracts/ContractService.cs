﻿using Application.Exceptions;
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
        _search.AddModule(new SimpleTextSearch(contract => contract.Name, weight: 5d));
        _search.AddModule(new SimpleTextSearch(contract => contract.SupplierName, weight: 1d));
        _search.AddModule(new BodyTextSearch(contract => contract.Description, weight: 1d));
        _search.AddModule(new BodyTextSearch(contract => contract.SupplierDescription, weight: 1d));
        _search.AddModule(new TagSearch { Weight = 5d, });
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
    public IEnumerable<Contract> Search(string query)
    {
        return _search.Search(query, FetchAllContracts());
    }

    /// <inheritdoc />
    public IEnumerable<ContractPreviewDto> SearchUnauthorized(string query)
    {
        IEnumerable<Contract> results = _search.Search(query, FetchAllContracts());
        return ConvertToPreviews(results);
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

    private static IEnumerable<ContractPreviewDto> ConvertToPreviews(IEnumerable<Contract> contracts)
    {
        return contracts.Select(contract => new ContractPreviewDto(contract));
    }
}
