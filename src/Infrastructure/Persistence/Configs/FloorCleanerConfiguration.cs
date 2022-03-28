using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal class FloorCleanerConfiguration : IEntityTypeConfiguration<FloorCleaner>
{
    public void Configure(EntityTypeBuilder<FloorCleaner> builder)
    {
        builder.HasKey(fc => new { fc.CleanerId, fc.FloorId });

        builder
            .HasOne(fc => fc.Floor)
            .WithMany(f => f.Cleaners)
            .HasForeignKey(fc => fc.FloorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(fc => fc.Cleaner)
            .WithMany(f => f.Workdays)
            .HasForeignKey(fc => fc.CleanerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
