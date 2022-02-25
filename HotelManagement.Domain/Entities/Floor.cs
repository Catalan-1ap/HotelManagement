using System.Collections.Generic;
using HotelManagement.Kernel;

namespace HotelManagement.Domain.Entities
{
	public sealed class Floor : IBaseEntity
	{
		public int Id { get; private set; }
		public int Number { get; set; }

		public ICollection<Room> Rooms { get; } = null!;
		public ICollection<FloorCleaner> Cleaners { get; } = null!;


		public Floor(int number)
		{
			Number = number;
		}


		// EF constructor
		internal Floor() { }
	}
}