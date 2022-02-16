namespace HotelManagement.Domain.Entities
{
	public class FloorCleaner
	{
		public int FloorId { get; set; }
		public Floor Floor { get; set; } = null!;

		public int CleanerId { get; set; }
		public Cleaner Cleaner { get; set; } = null!;

		public Weekday Weekday { get; set; }
	}
}