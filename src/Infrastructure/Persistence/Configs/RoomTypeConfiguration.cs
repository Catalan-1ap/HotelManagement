using Application.StorageContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder
            .Property(rt => rt.Description)
            .HasMaxLength(RoomTypeStorageContract.DescriptionMaxLength);

        builder.HasCheckConstraint(
            $"CK_{nameof(RoomType.MaxPeopleNumber)}",
            $"[{nameof(RoomType.MaxPeopleNumber)}] > 0");

        builder.HasCheckConstraint(
            $"CK_{nameof(RoomType.PricePerDay)}",
            $"[{nameof(RoomType.PricePerDay)}] >= 0");
    }
}
