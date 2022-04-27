using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.ClientEntity.CheckInClient;


public sealed class CheckInClientCommandHandler : IRequestHandler<CheckInClientCommand>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IApplicationDbContext _dbContext;


    public CheckInClientCommandHandler(IApplicationDbContext dbContext, IDateTimeService dateTimeService)
    {
        _dbContext = dbContext;
        _dateTimeService = dateTimeService;
    }


    public async Task<Unit> Handle(CheckInClientCommand request, CancellationToken token)
    {
        var client = await TryGetClient(request.Passport, token);
        var room = await TryGetRoom(request.RoomNumber, request.Passport, token);

        client.IsCheckout = false;
        client.City = request.City;
        client.Arrival = _dateTimeService.UtcNow;
        client.Room = room;

        await _dbContext.SaveChangesAsync(token);

        return Unit.Value;
    }


    private async Task<Client> TryGetClient(string passport, CancellationToken token)
    {
        var client = await _dbContext.Clients
            .Where(Client.CanCheckIn)
            .SingleOrDefaultAsync(c => c.Passport == passport, token);

        if (client is null)
            throw new NotFoundException(nameof(Client), passport);

        return client;
    }


    private async Task<Room> TryGetRoom(string roomNumber, string clientPassport, CancellationToken token)
    {
        var roomDetails = await _dbContext.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Clients)
            .Where(r => r.Number == roomNumber)
            .Select(r => new
            {
                Room = r,
                ResidentsNumber = r.Clients.Count,
                MaximumResidents = r.RoomType!.MaxPeopleNumber
            })
            .SingleOrDefaultAsync(token);

        if (roomDetails is null)
            throw new NotFoundException(nameof(Room), roomNumber);

        if (IsRoomHaveSpaceForClient(roomDetails.ResidentsNumber, roomDetails.MaximumResidents) == false)
            throw new RoomCrowdedException(roomNumber, clientPassport);

        return roomDetails.Room;
    }


    private bool IsRoomHaveSpaceForClient(int residentsNumber, int maximumResidents) => residentsNumber + 1 <= maximumResidents;
}
