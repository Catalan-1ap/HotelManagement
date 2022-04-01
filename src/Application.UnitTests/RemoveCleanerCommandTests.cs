using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.RemoveCleaner;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public class RemoveCleanerCommandTests : BaseCleanerTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly RemoveCleanerCommandHandler _handler;


    public RemoveCleanerCommandTests()
    {
        _dbContext = MakeContext();
        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldRemoveWhenExist()
    {
        // Arrange
        var added = await Add();
        var request = new RemoveCleanerCommand(added.Id);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Cleaners.Should().NotContain(added);
    }


    [Fact]
    public async Task ShouldThrowNotFoundIfDoesntExist()
    {
        // Arrange
        var request = new RemoveCleanerCommand(1);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task ShouldCallRemoveAndSaveChanges()
    {
        // Arrange
        var added = await Add();
        var request = new RemoveCleanerCommand(added.Id);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Cleaners.Received(Quantity.Exactly(1)).Remove(Arg.Any<Cleaner>());
        await _dbContext.Received(Quantity.Exactly(1)).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    private async Task<Cleaner> Add()
    {
        var context = MakeContext();

        context.Cleaners.Add(TestObject);
        await context.SaveChangesAsync(CancellationToken.None);

        return TestObject;
    }
}
