using System;
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
    private readonly IApplicationDbContext _dbContext;
    private readonly CheckOutClientCommandHandler _handler;
    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();


    public CheckOutClientComandTests()
    {
        _dbContext = MakeContext();

        _dateTimeService.UtcNow.Returns(new DateTime(2003, 2, 26));
        _handler = new(_dbContext, _dateTimeService);
    }


    [Fact]
    public async Task ShouldCheckOutWhenAllAlright()
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
        await CheckInClient(client.Passport, "City", room.Number);

        var elapsedDays = 4;
        _dateTimeService.UtcNow.Returns(_dateTimeService.UtcNow.AddDays(elapsedDays));
        var request = new CheckOutClientCommand(client.Passport);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var clientAssertion = _dbContext.Clients
            .Should()
            .ContainSingle(c => c.Passport == client.Passport)
            .Subject;
        clientAssertion.IsCheckout.Should().BeTrue();
        clientAssertion.Room.Should().BeNull();

        var report = _dbContext.RoomReports.Should().Contain(response).Subject;
        report.Client.Should()
            .NotBeNull()
            .And
            .Match<Client>(c => c.Passport == client.Passport);
        report.Room.Should()
            .NotBeNull()
            .And
            .Match<Room>(r => r.Number == room.Number);
        report.DaysNumber.Should().Be(elapsedDays);
        report.TotalPrice.Should().Be(room.RoomType.PricePerDay * elapsedDays);
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
            .Where(e => (string)e.Key == request.Passport);
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
