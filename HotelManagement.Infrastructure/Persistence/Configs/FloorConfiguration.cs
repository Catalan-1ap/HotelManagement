using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
    public sealed class FloorConfiguration : IEntityTypeConfiguration<Floor>
    {
        public void Configure(EntityTypeBuilder<Floor> builder) =>
            builder.HasCheckConstraint(
                $"CK_{nameof(Floor.Number)}",
                $"[{nameof(Floor.Number)}] > 0");
    }
}
