using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.CreateCleaner;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public class CreateCleanerCommandTests : BaseCleanerTestHandler
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
        var request = MakeCommand();

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await _dbContext.Cleaners.ContainsAsync(response, CancellationToken.None)).Should().BeTrue();
    }


    [Fact]
    public async Task ShouldCallAdd()
    {
        // Arrange
        var request = MakeCommand();

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Cleaners.Received(Quantity.Exactly(1)).Add(Arg.Any<Cleaner>());
    }


    [Fact]
    public async Task ShouldCallSaveChanges()
    {
        // Arrange
        var request = MakeCommand();

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _dbContext.Received(Quantity.Exactly(1)).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    private CreateCleanerCommand MakeCommand() =>
        new(
            TestObject.Person!.FirstName!,
            TestObject.Person.SurName!,
            TestObject.Person.Patronymic!);
}
