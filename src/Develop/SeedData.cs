using System;
using System.Linq;
using Domain.Entities;
using Infrastructure.Persistence;


namespace Develop;


public static class SeedData
{
    public static void Seed()
    {
        using var dbContext = ApplicationDbContextFactory.CreateDbContext();
        var random = new Random();

        SeedFloors(dbContext);
        SeedRoomTypes(dbContext);
        SeedRooms(dbContext, random, roomNumberPerFloor: (3, 6));
    }


    private static void SeedFloors(ApplicationDbContext dbContext)
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

        dbContext.Floors.AddRange(floors);
        dbContext.SaveChanges();
    }


    private static void SeedRoomTypes(ApplicationDbContext dbContext)
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

        dbContext.RoomTypes.AddRange(roomTypes);
        dbContext.SaveChanges();
    }


    private static void SeedRooms(ApplicationDbContext dbContext, Random random, (int min, int max) roomNumberPerFloor)
    {
        var roomTypes = dbContext.RoomTypes.ToArray();
        var floors = dbContext.Floors.ToArray();
        var (min, max) = roomNumberPerFloor;

        RoomType RandomRoomType()
        {
            var randomIndex = random.Next(roomTypes.Length);

            return roomTypes[randomIndex];
        }

        foreach (var floor in floors)
        {
            var roomNumber = random.Next(min, max);

            for (int i = 0; i < roomNumber; i++)
                dbContext.Rooms.Add(new()
                {
                    Number = $"{floor.Number}F-{i + 1}",
                    Floor = floor,
                    RoomType = RandomRoomType()
                });
        }

        dbContext.SaveChanges();
    }
}
