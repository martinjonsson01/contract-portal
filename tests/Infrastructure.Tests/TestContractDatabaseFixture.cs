using System.Diagnostics.CodeAnalysis;

using Infrastructure.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests;

public class TestContractDatabaseFixture
{
    private const string ConnectionString =
        @"Server=localhost;Database=master_test;User Id=SA; Password=ASDjk_shd$$jkASKJ19821!";

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
                context.Database.Migrate();
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
                .UseSqlServer(ConnectionString)
                .Options,
            Mock.Of<ILogger<EFContractRepository>>());
    }
}
