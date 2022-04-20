using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.RemoveCleaner;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Xunit;


namespace Application.UnitTests;


public class RemoveCleanerCommandTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly RemoveCleanerCommandHandler _handler;


    public RemoveCleanerCommandTests()
    {
        _dbContext = MakeContext();
        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldRemove_WhenExist()
    {
        // Arrange
        var cleaner = new Cleaner { Person = new() };

        await Add(cleaner);

        var request = new RemoveCleanerCommand(cleaner.Id);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Cleaners.Should().NotContain(cleaner);
    }


    [Fact]
    public async Task ShouldThrowNotFound_WhenDoesntExist()
    {
        // Arrange
        const int id = 1;
        var request = new RemoveCleanerCommand(id);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Cleaner))
            .Where(e => (int)e.Key == id);
    }


    private async Task Add(Cleaner cleaner)
    {
        var context = MakeContext();

        context.Cleaners.Add(cleaner);

        await context.SaveChangesAsync(CancellationToken.None);
    }
}
