using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configs;


internal sealed class FloorConfiguration : IEntityTypeConfiguration<Floor>
{
    public void Configure(EntityTypeBuilder<Floor> builder) =>
        builder.HasCheckConstraint(
            $"CK_{nameof(Floor.Number)}",
            $"[{nameof(Floor.Number)}] > 0");
}
