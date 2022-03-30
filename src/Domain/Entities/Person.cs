using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Person : IBaseEntity
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? SurName { get; set; }
    public string? Patronymic { get; set; }

    public ICollection<Client> Clients { get; } = new HashSet<Client>();
    public ICollection<Cleaner> Cleaners { get; } = new HashSet<Cleaner>();
}
