using System;
using Domain.Entities;


namespace Application.Exceptions;


public sealed class RoomCrowdedException : Exception
{
    public RoomCrowdedException(Room room, Client client)
        : base(
            $"Room with number {room.Number} is crowded, client with passport {client.Passport} cannot check-in in this room") { }
}