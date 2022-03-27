using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
    public sealed class RoomReportConfiguration : IEntityTypeConfiguration<RoomReport>
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
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
