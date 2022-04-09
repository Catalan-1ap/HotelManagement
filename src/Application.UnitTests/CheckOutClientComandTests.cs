using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.ClientEntity.CheckInClient;
using Application.CQRS.ClientEntity.CheckOutClient;
using Application.CQRS.ClientEntity.CreateClient;
using Application.Exceptions;
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

        _dateTimeService.UtcNow.Returns(new DateTime(2003, 2, 26));
        _handler = new(_dbContext, _dateTimeService);
    }


    [Fact]
    public async Task ShouldsetCorrectTotalPriceAndElapsedDays()
    {
        // Arrange
        var (room, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport);

        var elapsedDays = 4;
        _dateTimeService.UtcNow.Returns(_dateTimeService.UtcNow.AddDays(elapsedDays));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var report = _dbContext.RoomReports.Should().Contain(response).Subject;
        report.DaysNumber.Should().Be(elapsedDays);
        report.TotalPrice.Should().Be(room.RoomType!.PricePerDay * elapsedDays);
    }


    [Fact]
    public async Task ShouldCreateReportForPayerWithRoom()
    {
        // Arrange
        var (room, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var report = _dbContext.RoomReports.Should().Contain(response).Subject;
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
        var (room, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport);

        // Act
        var _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Clients.Should().OnlyContain(c => c.IsCheckout);
    }


    [Fact]
    public async Task ShouldSetRoomRelationshipToNull()
    {
        // Arrange
        var (room, clients) = await AddRoomWithClients();
        var payer = clients.First();
        var request = new CheckOutClientCommand(payer.Passport);

        // Act
        var _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Clients.Should().OnlyContain(c => c.Room == null);
    }


    [Fact]
    public async Task ShouldThrowWhenClientDoesntExists()
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
                MaxPeopleNumber = 1,
                PricePerDay = 10
            }
        };
        await AddRoom(room);
        var request = new CheckOutClientCommand(client.Passport);

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
            Number = "105",
            RoomType = new()
            {
                MaxPeopleNumber = 3,
                PricePerDay = 10
            }
        };
        await AddRoom(room);

        var clients = new List<Client>();
        clients.Add(new()
        {
            Passport = "Passport"
        });
        clients.Add(new()
        {
            Passport = "Passport 2"
        });
        clients.Add(new()
        {
            Passport = "Passport 3"
        });

        foreach (var client in clients)
        {
            await AddClient(client.Passport);
            await CheckInClient(client.Passport, "City", room.Number);
        }

        return (room, clients);
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


    private async Task CheckInClient(string passport, string city, string roomNumber)
    {
        var checkInCommand = new CheckInClientCommand(passport, city, roomNumber);

        var handler = new CheckInClientCommandHandler(MakeContext(), _dateTimeService);

        await handler.Handle(checkInCommand, CancellationToken.None);
    }
}
