using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Xml.Serialization;
using Xunit;

namespace Logic.Tests;
public class DBIntegrationTestBase : IDisposable, IAsyncDisposable
{
    private const string CONNECTION = "Host=postgres;Port=5432;Username=postgres;Password=admin;";
    private bool _disposed;

    protected readonly MyDbContext _myDbContext;

    protected DBIntegrationTestBase(string dbName)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(CONNECTION + $"Database={dbName}");

        using var dataSource = dataSourceBuilder.Build();

        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention()
            .EnableSensitiveDataLogging()
            .EnableServiceProviderCaching(false)
            .Options;

        _myDbContext = new MyDbContext(options);

        _myDbContext.Database.EnsureCreated();

        var connection = (NpgsqlConnection)_myDbContext.Database.GetDbConnection();

        if (connection.State is System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
        connection.ReloadTypes();
    }

    protected async Task<IList<T>> GetTableRecordsAsync<T>(
        string fromQuery,
        Func<DbDataReader, T> convertFunc)
    {
        using var cmd = _myDbContext.Database
            .GetDbConnection()
            .CreateCommand();

        cmd.CommandText = $"SELECT * FROM {fromQuery}";
        cmd.CommandType = System.Data.CommandType.Text;

        await _myDbContext.Database.OpenConnectionAsync()
            .ConfigureAwait(false);

        using var sqlResult = await cmd.ExecuteReaderAsync()
            .ConfigureAwait(false);

        var result = new List<T>();

        if (sqlResult.HasRows)
        {
            while (await sqlResult.ReadAsync().ConfigureAwait(false))
            {
                result.Add(convertFunc(sqlResult));
            }
        }

        await _myDbContext.Database.CloseConnectionAsync()
            .ConfigureAwait(false);

        return result;
    }

    protected async Task AddAsync(string fromQuery, string valuesQuery)
    {
        await using var cmd = _myDbContext.Database
            .GetDbConnection()
            .CreateCommand();

        cmd.CommandText = $"INSERT INTO {fromQuery} VALUES {valuesQuery}";
        cmd.CommandType = System.Data.CommandType.Text;

        await _myDbContext.Database
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await cmd.ExecuteNonQueryAsync()
            .ConfigureAwait(false);

        await _myDbContext.Database
            .CloseConnectionAsync()
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Task.Run(async () => await DisposeAsync().ConfigureAwait(false))
                .Wait();
        }
    }
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _myDbContext.Database.EnsureDeleted();

            await _myDbContext.DisposeAsync()
                .ConfigureAwait(false);

            _disposed = true;
        }
    }
}