using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.ClientEntity.CheckInClient;
using Application.CQRS.ClientEntity.CreateClient;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;


namespace Application.UnitTests;


public sealed class CheckInClientCommandTests : BaseClientTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly CheckInClientCommandHandler _handler;
    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();


    public CheckInClientCommandTests()
    {
        _dbContext = MakeContext();

        _dateTimeService.UtcNow.Returns(new DateTime(2003, 2, 26));
        _handler = new(_dbContext, _dateTimeService);
    }


    [Fact]
    public async Task ShouldCheckInWhenAllAlright()
    {
        // Arrange
        await AddClient();
        await AddRoom();
        var request = MakeCommand();

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var client = _dbContext.Clients.Should().Contain(c => c.Passport == request.Passport).Which;
        client.Arrival.Should().Be(_dateTimeService.UtcNow);
        client.IsCheckout.Should().BeFalse();
        client.Room.Should()
            .NotBeNull()
            .And
            .Match<Room>(r => r.Number == TestRoom.Number);
    }


    [Fact]
    public async Task ShouldThrowWhenRoomDoesntExists()
    {
        // Arrange
        await AddClient();
        var request = MakeCommand();

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Room))
            .Where(e => (string)e.Key == request.RoomNumber);
    }
    
    
    [Fact]
    public async Task ShouldThrowWhenClientDoesntExists()
    {
        // Arrange
        await AddRoom();
        var request = MakeCommand();

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Client))
            .Where(e => (string)e.Key == request.Passport);
    }
    
    
    [Fact]
    public async Task ShouldThrowWhenClientAlreadyCheckedIn()
    {
        // Arrange
        TestClient.IsCheckout = false;
        await AddClient();
        await AddRoom();
        var request = MakeCommand();

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<ClientAlreadyCheckedInException>())
            .Where(e => e.Passport == request.Passport);
    }
    
    
    [Fact]
    public async Task ShouldThrowWhenRoomIsCrowded()
    {
        // Arrange
        await AddClient();
        await AddRoom();
        await BindClientToRoom();
        
        TestClient.Passport = "NewPassport";
        await AddClient();
        var request = MakeCommand();
        
        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<RoomCrowdedException>())
            .Where(e => e.ClientPassport == request.Passport)
            .Where(e => e.RoomNumber == request.RoomNumber);
    }


    private async Task AddClient()
    {
        var dbContext = MakeContext();

        dbContext.Clients.Add(TestClient);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
    
    
    private async Task AddRoom()
    {
        var dbContext = MakeContext();

        dbContext.Rooms.Add(TestRoom);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
    
    
    private async Task BindClientToRoom()
    {
        var dbContext = MakeContext();

        var clients = await dbContext.Clients.ToListAsync();
        var room = await dbContext.Rooms.SingleAsync(r => r.Number == TestRoom.Number);
        
        foreach (var client in clients)
        {
            room.Clients.Add(client);
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }


    private CheckInClientCommand MakeCommand() =>
        new(
            TestClient.Passport,
            TestClient.City!,
            TestRoom.Number);
}
