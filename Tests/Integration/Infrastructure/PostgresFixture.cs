using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.Integration.Infrastructure;
public sealed class PostgresFixture : IAsyncLifetime
{
    private readonly string connectionString;

    public PostgresFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<PostgresFixture>()
            .Build();

        connectionString =
            configuration.GetConnectionString("CHATAPP_TEST_CONNECTION")
            ?? throw new InvalidOperationException(
                "TestDatabase connection string is missing from User Secrets.");
    }

    public ContextDb CreateContext()
    {
        var options = new DbContextOptionsBuilder<ContextDb>()
            .UseNpgsql(connectionString)
            .Options;

        return new ContextDb(options);
    }

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}