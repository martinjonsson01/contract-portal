using Application.Contracts;

using Domain.Contracts;

namespace Infrastructure.Contracts;

/// <summary>
/// Mocks fake contracts.
/// </summary>
public class FakeContractRepository : IContractRepository
{
    private readonly ICollection<Contract> _contracts;

    /// <summary>
    /// Creates a fake contract for SJ.
    /// </summary>
    public FakeContractRepository()
    {
        _contracts = new List<Contract> { new Contract() { Name = "SJ", ImagePath = "images/sj.png", }, };
    }

    /// <inheritdoc />
    public IEnumerable<Contract> All => new List<Contract>(_contracts);

    /// <inheritdoc />
    public void Add(Contract contract)
    {
        _contracts.Add(contract);
    }
}
