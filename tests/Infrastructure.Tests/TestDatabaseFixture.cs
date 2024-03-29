﻿using System;
using System.Diagnostics.CodeAnalysis;

using Application.Configuration;

using Infrastructure.Databases;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests;

public class TestDatabaseFixture
{
    private const string ConnectionString =
        @"Server=localhost;Database=ProdigoPortal_test;User Id=SA; Password=ASDjk_shd$$jkASKJ19821;";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
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
    public EFDatabaseContext CreateContext()
    {
        string connectionString = ConnectionString;

        string? ciContainer = Environment.GetEnvironmentVariable(ConfigurationKeys.ContinuousIntegration);
        if (ciContainer is null)
            connectionString += "Trusted_Connection=True;";

        return new EFDatabaseContext(
            new DbContextOptionsBuilder<EFDatabaseContext>()
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .Options,
            Mock.Of<ILogger<EFDatabaseContext>>(),
            Mock.Of<IConfiguration>());
    }
}
