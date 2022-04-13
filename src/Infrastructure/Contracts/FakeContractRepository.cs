﻿using System.Collections.ObjectModel;

using Application.Contracts;

using Domain.Contracts;

namespace Infrastructure.Contracts;

/// <summary>
/// Mocks fake contracts.
/// </summary>
public class FakeContractRepository : IContractRepository
{
    private readonly List<Contract> _contracts;
    private readonly IRecentContractService _recent;

    /// <summary>
    /// Creates a fake contract for SJ.
    /// </summary>
    public FakeContractRepository()
    {
        _contracts = new List<Contract> { new Contract() { Name = "SJ", ImagePath = "images/sj.png", }, };
        _recent = new RecentContractService(new Collection<Contract>());
    }

    /// <inheritdoc />
    public IEnumerable<Contract> All => new List<Contract>(_contracts);

    /// <inheritdoc />
    public IEnumerable<Contract> Recent => _recent.FetchRecentContracts();

    /// <inheritdoc />
    public void Add(Contract contract)
    {
        _contracts.Add(contract);
    }

    /// <inheritdoc />
    public void AddRecent(Contract contract)
    {
        _recent.AddRecent(contract);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        return _contracts.RemoveAll(o => o.Id == id) > 0;
    }
}
