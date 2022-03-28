using Application.StorageContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .Property(p => p.FirstName)
            .HasMaxLength(PersonStorageContract.FirstNameMaxLength);

        builder
            .Property(p => p.SurName)
            .HasMaxLength(PersonStorageContract.SurNameMaxLength);

        builder
            .Property(p => p.Patronymic)
            .HasMaxLength(PersonStorageContract.PatronymicMaxLength);
    }
}
