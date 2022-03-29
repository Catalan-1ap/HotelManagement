using System.Threading;
using System.Threading.Tasks;
using Application.CleanerCQRS.Commands.CreateCleaner;
using Application.Interfaces;
using Application.UnitTests.Common;
using Application.UnitTests.Mocks;
using Domain.Entities;
using FluentAssertions;
using MediatR.Wrappers;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public class CreateCleanerCommandTests : BaseTestHandler
{
    private readonly CreateCleanerCommandHandler _handler;


    public CreateCleanerCommandTests()
    {
        _handler = new(DbContext);
    }


    [Fact]
    public async Task ShouldCreateCleaner()
    {
        // Arrange
        var request = new CreateCleanerCommand("name", "surname", "patronymic");

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
        var request = new CreateCleanerCommand("name", "surname", "patronymic");

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        DbContext.Cleaners.Received(Quantity.Exactly(1)).Add(Arg.Any<Cleaner>());
    }


    [Fact]
    public async Task ShouldCallSaveChanges()
    {
        // Arrange
        var request = new CreateCleanerCommand("name", "surname", "patronymic");

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await DbContext.Received(Quantity.Exactly(1)).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
