using System.Data;

using Application.Configuration;
using Application.Contracts;
using Application.Users;

using Domain.Contracts;
using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Databases;

/// <summary>
/// Stores and fetches users from an Entity Framework Core database.
/// </summary>
public sealed class EFUserRepository : IUserRepository
{
    private const string AdminUserName = "admin";

    private readonly EFDatabaseContext _context;
    private readonly ILogger<EFUserRepository> _logger;
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFUserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    /// <param name="config">The configuration source.</param>
    public EFUserRepository(EFDatabaseContext context, ILogger<EFUserRepository> logger, IConfiguration config)
    {
        _context = context;
        _logger = logger;
        _config = config;
    }

    /// <inheritdoc />
    public IEnumerable<User> All => new List<User>(Users);

    private DbSet<User> Users => _context.Users;

    /// <inheritdoc />
    public void Add(User user)
    {
        _ = Users.Add(user);
        _ = _context.SaveChanges();
        _logger.LogInformation("Added a new user with name {Name} and id {Id} to the database", user.Name, user.Id);
    }

    /// <inheritdoc />
    public bool Remove(Guid id)
    {
        User? toRemove = Users.Find(id);
        if (toRemove is null)
            return false;

        _ = Users.Remove(toRemove);

        int changes = 0;
        try
        {
            changes = _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not remove contract from database: {Message}", e.Message);
        }

        // If any changes were made, then the remove operation succeeded.
        return changes > 0;
    }

    /// <inheritdoc />
    public User? Fetch(Guid id)
    {
        return Users.Include(user => user.Favorites).FirstOrDefault(user => user.Id == id);
    }

    /// <inheritdoc />
    public User? FromName(string username)
    {
        return Users.Include(user => user.Favorites).FirstOrDefault(user => user.Name == username);
    }

    /// <inheritdoc />
    public void AddFavorite(Guid userId, Guid contractId)
    {
        User? user = Fetch(userId);
        if (user is null)
            throw new UserDoesNotExistException(userId);

        Contract contract = FetchContract(contractId);

        user.Favorites.Add(contract);
        _ = _context.SaveChanges();
        _logger.LogInformation(
            "Contract {ContractId} was added as favorite for user with ID {UserId}",
            contractId,
            userId);
    }

    /// <inheritdoc />
    public bool RemoveFavorite(Guid userId, Guid contractId)
    {
        User? user = Fetch(userId);
        if (user is null)
            throw new UserDoesNotExistException(userId);

        Contract contract = FetchContract(contractId);
        bool result = user.Favorites.Remove(contract);
        _context.SaveChanges();

        _logger.LogInformation(
            "Contract {ContractId} was removed as favorite from user with ID {UserId}",
            contractId,
            userId);
        return result;
    }

    /// <summary>
    /// Updates user.
    /// </summary>
    /// <param name="updatedUser">Updates the values of an existing user.</param>
    public void UpdateUser(User updatedUser)
    {
        User? oldUser = Fetch(updatedUser.Id);
        if (oldUser is null)
            _ = Users.Add(updatedUser);
        else
            _context.Entry(oldUser).CurrentValues.SetValues(updatedUser);

        try
        {
            _ = _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not update user in database: {Message}", e.Message);
        }
    }

    /// <inheritdoc />
    public void EnsureAdminCreated()
    {
        if (!Users.Any(user => user.Name == AdminUserName))
            CreateAdmin();
    }

    private void CreateAdmin()
    {
        string? adminPasswordSecret = _config[ConfigurationKeys.AdminPassword];
        adminPasswordSecret = BCrypt.Net.BCrypt.HashPassword(adminPasswordSecret);
        var admin = new User
        {
            Name = AdminUserName,
            Password = adminPasswordSecret,
            Company = "Prodigo",
            LatestPaymentDate = DateTime.MaxValue,
        };
        _ = Users.Add(admin);
        try
        {
            _ = _context.SaveChanges();
        }
        catch (DataException e)
        {
            _logger.LogError("Could not create admin: {Message}", e.Message);
        }
    }

    private Contract FetchContract(Guid contractId)
    {
        try
        {
            return _context.Contracts.First(s => s.Id == contractId);
        }
        catch (InvalidOperationException)
        {
            throw new ContractDoesNotExistException();
        }
    }
}
