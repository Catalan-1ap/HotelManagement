using System;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
	public class Client : IBaseEntity
	{
		public int Id { get; set; }

		public string Passport { get; set; } = null!;
		public string City { get; set; } = null!;
		public DateTime Arrival { get; set; }

		public int PersonId { get; set; }
		public Person Person { get; set; } = null!;

		public int RoomId { get; set; }
		public Room Room { get; set; } = null!;
	}
}