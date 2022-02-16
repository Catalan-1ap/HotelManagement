using System.Collections.Generic;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
	public class Floor : IBaseEntity
	{
		public int Id { get; set; }

		public int Number { get; set; }

		public ICollection<Room> Rooms { get; private set; } = null!;
		public ICollection<FloorCleaner> Cleaners { get; private set; } = null!;


		public Floor(int number) { Number = number; }


		// EF constructor
		private Floor() { }
	}
}