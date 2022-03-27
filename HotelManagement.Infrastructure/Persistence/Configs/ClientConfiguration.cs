using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
    public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public static readonly int PassportMaxLength = 30;
        public static readonly int CityMaxLength = 30;


        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasQueryFilter(client => client.IsCheckout == false);
            
            builder
                .Property(c => c.Passport)
                .HasMaxLength(PassportMaxLength);
 
            builder
                .Property(c => c.City)
                .HasMaxLength(CityMaxLength);

            builder
                .HasOne(c => c.Person)
                .WithMany(p => p.Clients)
                .HasForeignKey(c => c.PersonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasOne(c => c.Room)
                .WithMany(r => r.Clients)
                .HasForeignKey(c => c.RoomId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
