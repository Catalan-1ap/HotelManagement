using Application.StorageContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class CleanerConfiguration : IEntityTypeConfiguration<Cleaner>
{
    public void Configure(EntityTypeBuilder<Cleaner> builder)
    {
        builder
            .Property(c => c.FirstName)
            .HasMaxLength(PersonStorageContract.FirstNameMaxLength);

        builder
            .Property(c => c.SurName)
            .HasMaxLength(PersonStorageContract.SurNameMaxLength);

        builder
            .Property(c => c.Patronymic)
            .HasMaxLength(PersonStorageContract.PatronymicMaxLength);
    }
}
