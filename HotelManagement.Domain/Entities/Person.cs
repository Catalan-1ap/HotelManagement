using System.Collections.Generic;
using HotelManagement.Kernel;


namespace HotelManagement.Domain.Entities
{
    public class Person : IBaseEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public string Patronymic { get; set; } = null!;


        public ICollection<Client> Clients { get; set; } = null!;
        public ICollection<Cleaner> Cleaners { get; set; } = null!;
    }
}
