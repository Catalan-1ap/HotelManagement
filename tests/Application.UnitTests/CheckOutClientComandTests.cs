using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.ClientEntity.CheckInClient;
using Application.Features.ClientEntity.CheckOutClient;
using Application.Features.ClientEntity.CreateClient;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;


namespace Application.UnitTests;


public sealed class CheckOutClientComandTests : BaseTestHandler
{
    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();
    private readonly IApplicationDbContext _dbContext;
    private readonly CheckOutClientCommandHandler _handler;


    public CheckOutClientComandTests()
    {
        _dbContext = MakeContext();

        _dateTimeService.Now.Returns(new DateTime(2003, 2, 26));
        _handler = new(_dbContext, _dateTimeService);
    }


    [Fact]
    public async Task ShouldProvideAdditionalInformation()
    {
        // Arrange
        var (room, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport!);

        const int elapsedDays = 4;
        _dateTimeService.Now.Returns(_dateTimeService.Now.AddDays(elapsedDays));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.DaysNumber.Should().Be(elapsedDays);
        response.TotalPrice.Should().Be(room.RoomType!.PricePerDay * elapsedDays);
        response.Party.Should().BeEquivalentTo(clients.Except(new[] { payer }));
        response.RoomNumber.Should().BeEquivalentTo(room.Number);
    }


    [Fact]
    public async Task ShouldCreateReportForPayerWithRoom()
    {
        // Arrange
        var (room, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport!);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var report = _dbContext.RoomReports.Should().Contain(report => report!.Client!.Passport == payer.Passport).Subject;
        report.Client.Should()
            .NotBeNull()
            .And
            .Match<Client>(c => c.Passport == payer.Passport);
        report.Room.Should()
            .NotBeNull()
            .And
            .Match<Room>(r => r.Number == room.Number);
    }


    [Fact]
    public async Task ShouldCheckoutPayerParty()
    {
        // Arrange
        var (_, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport!);

        // Act
        _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Clients.Should().OnlyContain(c => c.IsCheckout);
    }


    [Fact]
    public async Task ShouldSetRoomRelationshipToNull()
    {
        // Arrange
        var (_, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport!);

        // Act
        var _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Clients.Should().OnlyContain(c => c.Room == null);
    }


    [Fact]
    public async Task ShouldThrow_WhenClientDoesntExists()
    {
        // Arrange
        var client = new Client { Passport = "1" };
        var room = new Room
        {
            Number = "105",
            RoomType = new()
            {
                MaxPeopleNumber = 1,
                PricePerDay = 10
            }
        };

        client = await AddClient(client);
        await AddRoom(room);

        var request = new CheckOutClientCommand(client.Passport!);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Client))
            .Where(e => (string)e.Key == request.PayerPassport);
    }


    private async Task<(Room, IEnumerable<Client>)> AddRoomWithClients()
    {
        var room = new Room
        {
            Number = "1",
            RoomType = new()
            {
                MaxPeopleNumber = 3,
                PricePerDay = 10
            }
        };

        await AddRoom(room);

        var clients = new List<Client>
        {
            new()
            {
                Passport = "1"
            },
            new()
            {
                Passport = "2"
            },
            new()
            {
                Passport = "3"
            }
        };

        for (var index = 0; index < clients.Count; index++)
        {
            clients[index] = await AddClient(clients[index]);
            await CheckInClient(clients[index], room);
        }

        return (room, clients);
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


    private async Task CheckInClient(Client client, Room room)
    {
        var checkInCommand = new CheckInClientCommand(client.Passport!, "City", room.Number!);

        var handler = new CheckInClientCommandHandler(MakeContext(), _dateTimeService);

        await handler.Handle(checkInCommand, CancellationToken.None);
    }
}
