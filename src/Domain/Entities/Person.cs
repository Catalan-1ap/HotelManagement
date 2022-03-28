using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Person : IBaseEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string SurName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;

    public ICollection<Client> Clients { get; } = null!;
    public ICollection<Cleaner> Cleaners { get; } = null!;
}
