using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class RoomReportConfiguration : IEntityTypeConfiguration<RoomReport>
{
    public void Configure(EntityTypeBuilder<RoomReport> builder)
    {
        builder
            .HasOne(r => r.Room)
            .WithMany(r => r.Reports)
            .HasForeignKey(r => r.RoomId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.Client)
            .WithMany(c => c.Visits)
            .HasForeignKey(c => c.ClientId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
