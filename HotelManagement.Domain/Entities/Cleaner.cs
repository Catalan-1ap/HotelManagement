using System.Collections.Generic;

namespace HotelManagement.Domain.Entities
{
	public sealed class Cleaner : Person
	{
		public ICollection<FloorCleaner> Workdays { get; } = null!;


		// EF constructor
		internal Cleaner() { }
	}
}