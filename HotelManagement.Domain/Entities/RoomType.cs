using Ardalis.SmartEnum;


namespace HotelManagement.Domain.Entities
{
	public class RoomType : SmartEnum<RoomType>
	{
		public static readonly RoomType Single = new("Одноместный", 1)
		{
			PeopleNumber = 1, PricePerDay = 1000
		};
		public static readonly RoomType Double = new("Двуместный", 2)
		{
			PeopleNumber = 2, PricePerDay = 2000
		};
		public static readonly RoomType Triple = new("Трехместный", 3)
		{
			PeopleNumber = 3, PricePerDay = 3000
		};

		public int PeopleNumber { get; init; }
		public int PricePerDay { get; init; }


		public RoomType(string name, int value) : base(name, value) { }
	}
}