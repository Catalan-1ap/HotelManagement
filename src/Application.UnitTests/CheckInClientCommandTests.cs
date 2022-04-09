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
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;


namespace Application.UnitTests;


public sealed class CheckInClientCommandTests : BaseTestHandler
{
    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();
    private readonly IApplicationDbContext _dbContext;
    private readonly CheckInClientCommandHandler _handler;


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
        var client = new Client
        {
            Passport = "Passport"
        };
        await AddClient(client.Passport);
        var room = new Room
        {
            Number = "105",
            RoomType = new()
            {
                MaxPeopleNumber = 1
            }
        };
        await AddRoom(room);
        var request = new CheckInClientCommand(client.Passport, "City", room.Number);

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        client = _dbContext.Clients.Should().Contain(c => c.Passport == request.Passport).Which;
        client.Arrival.Should().Be(_dateTimeService.UtcNow);
        client.City.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(request.City);
        client.IsCheckout.Should().BeFalse();
        client.Room.Should()
            .NotBeNull()
            .And
            .Match<Room>(r => r.Number == room.Number);
    }


    [Fact]
    public async Task ShouldThrowWhenRoomDoesntExists()
    {
        // Arrange
        var client = new Client
        {
            Passport = "Passport"
        };
        await AddClient(client.Passport);
        var request = new CheckInClientCommand(client.Passport, "City", "105");

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
        var room = new Room
        {
            Number = "105"
        };
        await AddRoom(room);
        var request = new CheckInClientCommand("Passport", "City", room.Number);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Client))
            .Where(e => (string)e.Key == request.Passport);
    }


    [Fact]
    public async Task ShouldThrowWhenRoomIsCrowded()
    {
        // Arrange
        var client = new Client
        {
            Passport = "Passport"
        };
        await AddClient(client.Passport);
        var room = new Room
        {
            Number = "105",
            RoomType = new()
            {
                MaxPeopleNumber = 1
            }
        };
        await AddRoom(room);
        await BindClientToRoom(room.Number);

        client.Passport = "NewPassport";
        await AddClient(client.Passport);
        var request = new CheckInClientCommand(client.Passport, "City", room.Number);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<RoomCrowdedException>())
            .Where(e => e.ClientPassport == request.Passport)
            .Where(e => e.RoomNumber == request.RoomNumber);
    }


    private async Task AddClient(string passport)
    {
        var addCommand = new CreateClientCommand(passport,
            new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            });
        var handler = new CreateClientCommandHandler(MakeContext());

        await handler.Handle(addCommand, CancellationToken.None);
    }


    private async Task AddRoom(Room room)
    {
        var dbContext = MakeContext();

        dbContext.Rooms.Add(room);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }


    private async Task BindClientToRoom(string roomNumber)
    {
        var dbContext = MakeContext();

        var clients = await dbContext.Clients.ToListAsync();
        var room = await dbContext.Rooms.SingleAsync(r => r.Number == roomNumber);

        foreach (var client in clients)
            room.Clients.Add(client);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
