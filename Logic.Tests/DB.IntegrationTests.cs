using FluentAssertions;
using Xunit;

namespace Logic.Tests;

public class DBIntegrationTests : DBIntegrationTestBase
{
    public DBIntegrationTests() 
        : base(nameof(DBIntegrationTests))
    {
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var model = new SomeModel
        {
            Id = 1,
            Name = "Foo",
        };

        // Act
        using var transaction = _myDbContext.Database.BeginTransaction();

        _myDbContext.SomeModels.Add(model);
        await _myDbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        var records = await GetTableRecordsAsync(
            "some_model_table",
            (reader) => new SomeModel
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });

        // Assert
        records.Single().Should().BeEquivalentTo(model);
    }
}