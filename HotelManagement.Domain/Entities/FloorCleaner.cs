namespace HotelManagement.Domain.Entities
{
    public sealed class FloorCleaner
    {
        public Weekday Weekday { get; set; }
        
        public int FloorId { get; set; }
        public Floor Floor { get; set; } = null!;

        public int CleanerId { get; set; }
        public Cleaner Cleaner { get; set; } = null!;
    }
}
