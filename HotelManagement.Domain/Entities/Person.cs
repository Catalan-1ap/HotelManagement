using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
	public class Person : IBaseEntity
	{
		public int Id { get; set; }

		public string FirstName { get; set; } = null!;
		public string SurName { get; set; } = null!;
		public string Patronymic { get; set; } = null!;


		public Person(string firstName, string surName, string patronymic)
		{
			FirstName  = firstName;
			SurName    = surName;
			Patronymic = patronymic;
		}


		// EF constructor
		private Person() { }
	}
}