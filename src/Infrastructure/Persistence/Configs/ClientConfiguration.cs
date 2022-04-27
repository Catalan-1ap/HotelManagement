using Application.StorageContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Passport);

        builder
            .Property(c => c.Passport)
            .HasMaxLength(ClientStorageContract.PassportMaxLength);

        builder
            .Property(c => c.City)
            .HasMaxLength(ClientStorageContract.CityMaxLength);

        builder
            .Property(c => c.FirstName)
            .HasMaxLength(PersonStorageContract.FirstNameMaxLength);

        builder
            .Property(c => c.SurName)
            .HasMaxLength(PersonStorageContract.SurNameMaxLength);

        builder
            .Property(c => c.Patronymic)
            .HasMaxLength(PersonStorageContract.PatronymicMaxLength);

        builder
            .HasOne(c => c.Room)
            .WithMany(r => r.Clients)
            .HasForeignKey(c => c.RoomId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
