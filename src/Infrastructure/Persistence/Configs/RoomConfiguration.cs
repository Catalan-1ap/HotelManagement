using Application.StorageContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder
            .HasKey(r => r.Number);

        builder
            .Property(r => r.Number)
            .HasMaxLength(RoomStorageContract.NumberMaxLength);

        builder
            .HasOne(r => r.RoomType)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(r => r.RoomTypeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(r => r.Floor)
            .WithMany(f => f.Rooms)
            .HasForeignKey(r => r.FloorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
