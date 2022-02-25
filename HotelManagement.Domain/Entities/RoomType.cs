using System.Collections.Generic;
using HotelManagement.Kernel;

namespace HotelManagement.Domain.Entities
{
	public sealed class RoomType : IBaseEntity
	{
		public int Id { get; private set; }
		public string Description { get; init; } = null!;
		public int PeopleNumber { get; init; }
		public int PricePerDay { get; set; }


		public ICollection<Room> Rooms { get; private set; }


		// EF Constructor
		internal RoomType() { }
	}
}