using FluentAssertions;
using Xunit;

namespace Logic.Tests;

public class WorkerTests
{
    [Fact]
    public async void Test1()
    {
        // Arrange
        var worker = new Worker();

        // Act
        var actual = await worker.DoWorkAsync();

        // Assert
        actual.Should().Be("WORKER DO WORK");
    }
}