using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
    public sealed class CleanerConfiguration : IEntityTypeConfiguration<Cleaner>
    {
        public void Configure(EntityTypeBuilder<Cleaner> builder)
        {
            builder
                .HasOne(c => c.Person)
                .WithMany(p => p.Cleaners)
                .HasForeignKey(c => c.PersonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
