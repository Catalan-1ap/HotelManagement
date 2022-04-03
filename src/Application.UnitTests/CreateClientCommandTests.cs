using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.ClientEntity.CreateClient;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public sealed class CreateClientCommandTests : BaseTestHandler
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
        var request = new CreateClientCommand("Passport",
            new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            });

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await _dbContext.Clients.IgnoreQueryFilters().ContainsAsync(response, CancellationToken.None)).Should()
            .BeTrue();
    }


    [Fact]
    public async Task ShouldCallAdd()
    {
        // Arrange
        var request = new CreateClientCommand("Passport",
            new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            });

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Clients.Received(Quantity.Exactly(1)).Add(Arg.Any<Client>());
    }


    [Fact]
    public async Task ShouldCallSaveChanges()
    {
        // Arrange
        var request = new CreateClientCommand("Passport",
            new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            });

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _dbContext.Received(Quantity.Exactly(1)).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
