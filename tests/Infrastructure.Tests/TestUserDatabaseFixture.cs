using System.Diagnostics.CodeAnalysis;

using Infrastructure.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests;

public class TestUserDatabaseFixture
{
    private const string ConnectionString =
        @"User ID=postgres;Password=password;Host=localhost;Port=5432;Database=user_portal_test;";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestUserDatabaseFixture()
    {
        lock (_lock)
        {
            if (_databaseInitialized)
                return;

            using (DbContext context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            _databaseInitialized = true;
        }
    }

    [SuppressMessage(
        "Performance",
        "CA1822:Mark members as static",
        Justification = "Needs to be non-static so each test instance can call it without the entire class being static.")]
    public EFUserRepository CreateContext()
    {
        return new EFUserRepository(
            new DbContextOptionsBuilder<EFUserRepository>()
                .UseNpgsql(ConnectionString)
                .Options,
            Mock.Of<ILogger<EFUserRepository>>());
    }
}
