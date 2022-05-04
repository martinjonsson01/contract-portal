﻿using System;
using System.Diagnostics.CodeAnalysis;
using Application.Contracts;
using Infrastructure.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests;

public class TestContractDatabaseFixture
{
    private const string ConnectionString =
        @"Server=localhost;Database=master_test_contract;User Id=SA; Password=ASDjk_shd$$jkASKJ19821;";

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
        string connectionString = ConnectionString;

        string? untrusted = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.UntrustedConnection);
        if (untrusted is null)
            connectionString += "Trusted_Connection=True;";

        return new EFContractRepository(
            new DbContextOptionsBuilder<EFContractRepository>()
                .UseSqlServer(connectionString)
                .Options,
            Mock.Of<ILogger<EFContractRepository>>(),
            Mock.Of<IRecentContractService>());
    }
}
