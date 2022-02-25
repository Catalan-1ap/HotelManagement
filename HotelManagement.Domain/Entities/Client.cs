using System;

namespace HotelManagement.Domain.Entities
{
	public sealed class Client : Person
	{
		public string Passport { get; set; } = null!;
		public string City { get; set; } = null!;
		public DateTime Arrival { get; set; }

		public int? RoomId { get; set; }
		public Room Room { get; set; } = null!;


		public Client(string passport, string city, DateTime arrival, Room room)
		{
			Passport = passport;
			City = city;
			Arrival = arrival;
			Room = room;
		}


		// EF Constructor
		internal Client() { }
	}
}