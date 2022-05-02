using System.Diagnostics.CodeAnalysis;

using Infrastructure.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests;

public class TestUserDatabaseFixture
{
    private const string ConnectionString =
        @"Server=localhost;Database=master_test_user;Trusted_Connection=True;User Id=SA; Password=ASDjk_shd$$jkASKJ19821";

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
                .UseSqlServer(ConnectionString)
                .Options,
            Mock.Of<ILogger<EFUserRepository>>());
    }
}
