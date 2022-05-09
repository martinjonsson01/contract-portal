using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;

namespace Application.FavoriteContracts;

/// <inheritdoc />
public class FavoriteContractService : IFavoriteContractService
{
    private readonly IFavoriteContractRepository _favoriteRepo;
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Constructs favorite service.
    /// </summary>
    /// <param name="favoriteRepo">Where to store and fetch favorites from.</param>
    /// <param name="userRepo">Where to store and fetch users from.</param>
    public FavoriteContractService(IFavoriteContractRepository favoriteRepo, IUserRepository userRepo)
    {
        _favoriteRepo = favoriteRepo;
        _userRepo = userRepo;
    }

    /// <inheritdoc />
    public void Add(string userName, Guid contractId)
    {
        _favoriteRepo.Add(userName, contractId);
    }

    /// <inheritdoc/>
    public bool Remove(string userName, Guid contractId)
    {
        return _favoriteRepo.Remove(userName, contractId);
    }

    /// <inheritdoc />
    public bool IsFavorite(string userName, Guid contractId)
    {
        User user = FetchUser(userName);
        return user.Contracts.Any(c => c.Id == contractId);
    }

    /// <inheritdoc/>
    public IEnumerable<Contract> FetchAll(string userName)
    {
        User user = FetchUser(userName);

        List<Contract> contractsWithoutSelfReference = new();

        foreach (Contract? contract in user.Contracts)
        {
            contract.Users = null!;
            contractsWithoutSelfReference.Add(contract);
        }

        return contractsWithoutSelfReference;
    }

    private User FetchUser(string username)
    {
        User? user = _userRepo.Fetch(username);

        if (user == null)
            throw new UserDoesNotExistException(username);

        return user;
    }
}
