using System.Threading;
using System.Threading.Tasks;
using Application.CleanerCQRS.Commands.CreateCleaner;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public class CreateCleanerCommandTests : BaseCleanerTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly CreateCleanerCommandHandler _handler;

    private CreateCleanerCommand MakeCommand() => new(
        TestObject.Person!.FirstName!,
        TestObject.Person.SurName!,
        TestObject.Person.Patronymic!);


    public CreateCleanerCommandTests() : base(nameof(CreateCleanerCommand))
    {
        _dbContext = GetContext();

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
        response.Id.Should().BeGreaterThan(0);
        response.Person.Should().NotBeNull();
        response.Person.Should().BeEquivalentTo(request);
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
}
