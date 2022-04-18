﻿using Application.Exceptions;

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
        return string.IsNullOrEmpty(query) ? FetchAllContracts() : new List<Contract>();
    }
}
