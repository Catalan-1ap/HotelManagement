using System.Collections.Generic;


namespace HotelManagement.Domain.Entities
{
    public sealed class Cleaner
    {
        public int Id { get; set; }
        
        
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
        
        
        public ICollection<FloorCleaner> Workdays { get; set; } = null!;
    }
}
