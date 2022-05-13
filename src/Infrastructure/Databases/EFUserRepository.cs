using System.Data;
using Application.Contracts;
using Application.Users;
using Domain.Contracts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="EFUserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to manipulate data in.</param>
    /// <param name="logger">The logging service to use.</param>
    public EFUserRepository(EFDatabaseContext context, ILogger<EFUserRepository> logger)
    {
        _context = context;
        _logger = logger;
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
    public bool Exists(string username)
    {
        return Users.Any(user => user.Name == username);
    }

    /// <inheritdoc />
    public User? Fetch(string username)
    {
        return Users.FirstOrDefault(user => user.Name == username);
    }

    /// <inheritdoc />
    public void EnsureAdminCreated()
    {
        if (!Users.Any(user => user.Name == AdminUserName))
            CreateAdmin();
    }

    private void CreateAdmin()
    {
        var admin = new User { Name = AdminUserName, Company = "Prodigo", LatestPaymentDate = DateTime.MaxValue };
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
}
