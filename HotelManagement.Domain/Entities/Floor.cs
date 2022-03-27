using System.Collections.Generic;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
    public sealed class Floor : IBaseEntity
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public ICollection<Room> Rooms { get; set; } = null!;
        public ICollection<FloorCleaner> Cleaners { get; set; } = null!;
    }
}
