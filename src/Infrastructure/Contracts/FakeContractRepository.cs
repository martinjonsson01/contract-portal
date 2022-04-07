using Application.Contracts;

using Domain.Contracts;

namespace Infrastructure.Contracts;

/// <summary>
/// Mocks fake contracts.
/// </summary>
public class FakeContractRepository : IContractRepository
{
    /// <summary>
    /// Creates a fake contract for SJ.
    /// </summary>
    public FakeContractRepository()
    {
        Contracts = new List<Contract>() { new Contract() { Name = "SJ", ImagePath = "images/sj.png", }, };
    }

    /// <inheritdoc />
    public ICollection<Contract> Contracts { get; }
}
