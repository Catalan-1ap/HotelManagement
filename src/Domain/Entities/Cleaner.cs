using System.Collections.Generic;


namespace Domain.Entities;


public sealed class Cleaner
{
    public int Id { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }

    public ICollection<CleaningSchedule> Workdays { get; } = new HashSet<CleaningSchedule>();
}
