using System.Collections.Generic;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
	public class Cleaner : IBaseEntity
	{
		public int Id { get; set; }

		public int PersonId { get; set; }
		public Person Person { get; set; } = null!;

		public ICollection<FloorCleaner> Workdays { get; private set; } = null!;


		public Cleaner(Person person) { Person = person; }


		// EF constructor
		private Cleaner() { }
	}
}