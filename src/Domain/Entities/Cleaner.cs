using System.Collections.Generic;


namespace Domain.Entities;


public sealed class Cleaner
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;

    public ICollection<CleaningSchedule> Workdays { get; } = new HashSet<CleaningSchedule>();
}
