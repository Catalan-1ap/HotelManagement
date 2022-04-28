using System.Collections.Generic;


namespace Domain.Entities;


public class Cleaner
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;

    public List<CleaningSchedule> Workdays { get; set; } = new List<CleaningSchedule>();
}
