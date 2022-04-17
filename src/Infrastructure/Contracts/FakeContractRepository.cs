using System.Collections.ObjectModel;

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
        _contracts = new List<Contract> { new Contract() { Name = "SJ", SupplierLogoImagePath = "images/sj.png", IsFavorite = true, }, };
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
        _recent.Add(contract);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        _recent.Remove(id); // This line should not exist when there is an actual database (it will remove any relations)
        return _contracts.RemoveAll(o => o.Id == id) > 0;
    }

    /// <inheritdoc />
    public IEnumerable<Contract> Favorites()
    {
        return _contracts.FindAll(contract => contract.IsFavorite);
    }

    /// <inheritdoc />
    public Contract? FetchContract(Guid id)
    {
        return _contracts.Find(contract => contract.Id == id);
    }

    /// <inheritdoc />
    public void UpdateContract(Contract updatedContract)
    {
        int index = _contracts.FindIndex(contract => contract.Id == updatedContract.Id);
        if (index != -1)
            _contracts[index] = updatedContract;
    }
}
