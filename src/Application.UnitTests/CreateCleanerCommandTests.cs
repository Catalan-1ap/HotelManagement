using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.CreateCleaner;
using Application.Interfaces;
using Application.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Application.UnitTests;


public class CreateCleanerCommandTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly CreateCleanerCommandHandler _handler;


    public CreateCleanerCommandTests()
    {
        _dbContext = MakeContext();

        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldCreateCleaner()
    {
        // Arrange
        var request = new CreateCleanerCommand("F", "S", "P");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await _dbContext.Cleaners.ContainsAsync(response, CancellationToken.None)).Should().BeTrue();
    }
}
