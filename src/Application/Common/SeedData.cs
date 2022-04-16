using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Common;


public sealed class SeedData
{
    private readonly IApplicationDbContext _dbContext;
    private readonly Random _random = new();


    public SeedData(IApplicationDbContext dbContext) => _dbContext = dbContext;


    public async Task Seed()
    {
        SeedFloors();
        SeedRoomTypes();
        await SeedRooms(roomNumberPerFloor: (3, 6));

        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }


    private void SeedFloors()
    {
        var floors = new Floor[]
        {
            new()
            {
                Number = 1
            },
            new()
            {
                Number = 2
            },
            new()
            {
                Number = 3
            }
        };

        _dbContext.Floors.AddRange(floors);
    }


    private void SeedRoomTypes()
    {
        var roomTypes = new RoomType[]
        {
            new()
            {
                Description = "Одноместный номер",
                MaxPeopleNumber = 1,
                PricePerDay = 1500
            },
            new()
            {
                Description = "Двуместный номер",
                MaxPeopleNumber = 2,
                PricePerDay = 2400
            }
        };

        _dbContext.RoomTypes.AddRange(roomTypes);
    }


    private async Task SeedRooms((int min, int max) roomNumberPerFloor)
    {
        var roomTypes = await _dbContext.RoomTypes.ToArrayAsync();
        var floors = await _dbContext.Floors.ToArrayAsync();
        var (min, max) = roomNumberPerFloor;

        RoomType RandomRoomType()
        {
            var randomIndex = _random.Next(roomTypes.Length);

            return roomTypes[randomIndex];
        }

        foreach (var floor in floors)
        {
            var roomNumber = _random.Next(min, max);

            for (int i = 0; i < roomNumber; i++)
                _dbContext.Rooms.Add(new()
                {
                    Number = $"{floor.Number}F-{i + 1}",
                    Floor = floor,
                    RoomType = RandomRoomType()
                });
        }
    }
}
