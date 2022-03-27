using System.Collections.Generic;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
    public sealed class Room : IBaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;

        
        public int? RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;

        public int FloorId { get; set; }
        public Floor Floor { get; set; } = null!;

        
        public ICollection<Client> Clients { get; set; } = null!;
        public ICollection<RoomReport> Reports { get; set; } = null!;
    }
}
