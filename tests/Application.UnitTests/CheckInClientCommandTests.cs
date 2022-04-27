using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.ClientEntity.CheckInClient;
using Application.Features.ClientEntity.CreateClient;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
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
    public async Task ShouldCheckIn_WhenAllAlright()
    {
        // Arrange
        var client = new Client { Passport = "1" };
        var room = new Room
        {
            Number = "1",
            RoomType = new()
            {
                MaxPeopleNumber = 1
            }
        };

        client = await AddClient(client);
        await AddRoom(room);

        var request = new CheckInClientCommand(client.Passport!, "City", room.Number!);

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
    public async Task ShouldThrow_WhenRoomDoesntExists()
    {
        // Arrange
        var client = new Client { Passport = "1" };

        client = await AddClient(client);

        var request = new CheckInClientCommand(client.Passport!, "City", "105");

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Room))
            .Where(e => (string)e.Key == request.RoomNumber);
    }


    [Fact]
    public async Task ShouldThrow_WhenClientDoesntExists()
    {
        // Arrange
        var room = new Room { Number = "1" };

        await AddRoom(room);

        var request = new CheckInClientCommand("Passport", "City", room.Number!);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Client))
            .Where(e => (string)e.Key == request.Passport);
    }


    [Fact]
    public async Task ShouldThrow_WhenRoomIsCrowded()
    {
        // Arrange
        var checkedClient = new Client
        {
            Passport = "1"
        };
        var nonCheckedClient = new Client
        {
            Passport = "2"
        };
        var room = new Room
        {
            Number = "1",
            RoomType = new()
            {
                MaxPeopleNumber = 1
            }
        };

        checkedClient = await AddClient(checkedClient);
        nonCheckedClient = await AddClient(nonCheckedClient);
        await AddRoom(room);
        await BindClientsToRoom(room, checkedClient);

        var request = new CheckInClientCommand(nonCheckedClient.Passport!, "City", room.Number!);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<RoomCrowdedException>())
            .Where(e => e.ClientPassport == request.Passport)
            .Where(e => e.RoomNumber == request.RoomNumber);
    }


    private async Task<Client> AddClient(Client client)
    {
        var addCommand = new CreateClientCommand(client.Passport!, string.Empty, string.Empty, string.Empty);
        var handler = new CreateClientCommandHandler(MakeContext());

        return await handler.Handle(addCommand, CancellationToken.None);
    }


    private async Task AddRoom(Room room)
    {
        var dbContext = MakeContext();

        dbContext.Rooms.Add(room);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }


    private async Task BindClientsToRoom(Room room, params Client[] clients)
    {
        var dbContext = MakeContext();
        dbContext.Rooms.Attach(room);

        foreach (var client in clients)
        {
            dbContext.Clients.Attach(client);
            room.Clients.Add(client);
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
