using System.Collections.Generic;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
	public class Room : IBaseEntity
	{
		public int Id { get; set; }

		public string Number { get; set; } = null!;
		public RoomType RoomType { get; set; } = null!;
		

		public int FloorId { get; set; }
		public Floor Floor { get; set; } = null!;


		public ICollection<Client> Clients { get; private set; } = null!;


		public Room(string number, Floor floor, RoomType roomType)
		{
			Number   = number;
			Floor    = floor;
			RoomType = roomType;
		}


		// EF constructor
		private Room() { }
	}
}