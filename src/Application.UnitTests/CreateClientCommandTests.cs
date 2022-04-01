using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.CreateCleaner;
using Application.CQRS.ClientEntity.CreateClient;
using Application.Interfaces;
using Application.UnitTests.Common;
using Application.UnitTests.Mocks;
using Domain.Entities;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public sealed class CreateClientCommandTests : BaseClientTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly CreateClientCommandHandler _handler;


    public CreateClientCommandTests()
    {
        _dbContext = MakeContext();

        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldCreate()
    {
        // Arrange
        var request = MakeCommand();

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await _dbContext.Clients.IgnoreQueryFilters().ContainsAsync(response, CancellationToken.None)).Should().BeTrue();
    }


    [Fact]
    public async Task ShouldCallAdd()
    {
        // Arrange
        var request = MakeCommand();

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Clients.Received(Quantity.Exactly(1)).Add(Arg.Any<Client>());
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


    private CreateClientCommand MakeCommand() => new(
        TestObject.Passport,
        TestObject.City!,
        TestObject.Person!);
}
