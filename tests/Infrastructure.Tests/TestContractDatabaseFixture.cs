using System.Diagnostics.CodeAnalysis;

using Infrastructure.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests;

public class TestContractDatabaseFixture
{
    private const string ConnectionString =
        @"User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal_test;";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestContractDatabaseFixture()
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
    public EFContractRepository CreateContext()
    {
        return new EFContractRepository(
            new DbContextOptionsBuilder<EFContractRepository>()
                .UseNpgsql(ConnectionString)
                .Options,
            Mock.Of<ILogger<EFContractRepository>>());
    }
}
