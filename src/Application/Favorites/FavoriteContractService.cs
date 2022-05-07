using Domain.Contracts;

namespace Application.FavoriteContracts;

/// <inheritdoc />
public class FavoriteContractService : IFavoriteContractService
{
    private readonly IFavoriteContractRepository _repo;

    /// <summary>
    /// Constructs user service.
    /// </summary>
    /// <param name="repo">Where to store and fetch favorites from.</param>
    public FavoriteContractService(IFavoriteContractRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public void Add(string userName, Guid contractId)
    {
        _repo.Add(userName, contractId);
    }

    /// <inheritdoc />
    public bool CheckIfFavorite(string userName, Guid contractId)
    {
        return _repo.CheckIfFavorite(userName, contractId);
    }

    /// <inheritdoc/>
    public IEnumerable<Contract> FetchAllFavorites(string userName)
    {
        return _repo.FetchAllFavorites(userName);
    }

    /// <inheritdoc/>
    public bool Remove(string userName, Guid contractId)
    {
        return _repo.Remove(userName, contractId);
    }
}
