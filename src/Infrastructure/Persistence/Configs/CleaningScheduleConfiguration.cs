using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal class CleaningScheduleConfiguration : IEntityTypeConfiguration<CleaningSchedule>
{
    public void Configure(EntityTypeBuilder<CleaningSchedule> builder)
    {
        builder.HasKey(fc => new { fc.FloorId, fc.Weekday });

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
