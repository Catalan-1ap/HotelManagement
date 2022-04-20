using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.ClientEntity.CreateClient;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
    public async Task ShouldCreate_WhenAllAlright()
    {
        // Arrange
        var request = new CreateClientCommand("1", new());

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await _dbContext.Clients.ContainsAsync(response, CancellationToken.None)).Should()
            .BeTrue();
    }


    [Fact]
    public async Task ShouldThrow_WhenSamePassportExists()
    {
        // Arrange
        var request = new CreateClientCommand("1", new());
        await _handler.Handle(request, CancellationToken.None);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ClientWithPassportAlreadyExistsException>()
            .Where(e => e.Passport == request.Passport);
    }
}
