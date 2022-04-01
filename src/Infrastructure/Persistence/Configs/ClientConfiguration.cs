using Application.StorageContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasQueryFilter(client => client.IsCheckout == false);

        builder.HasKey(c => c.Passport);

        builder
            .Property(c => c.Passport)
            .HasMaxLength(ClientStorageContract.PassportMaxLength);

        builder
            .Property(c => c.City)
            .HasMaxLength(ClientStorageContract.CityMaxLength);

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
