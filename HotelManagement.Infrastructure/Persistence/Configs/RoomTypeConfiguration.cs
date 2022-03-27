using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
    public sealed class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public static readonly int DescriptionMaxLength = 80;


        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder
                .Property(rt => rt.Description)
                .HasMaxLength(DescriptionMaxLength);

            builder.HasCheckConstraint(
                $"CK_{nameof(RoomType.PeopleNumber)}",
                $"[{nameof(RoomType.PeopleNumber)}] > 0");

            builder.HasCheckConstraint(
                $"CK_{nameof(RoomType.PricePerDay)}",
                $"[{nameof(RoomType.PricePerDay)}] >= 0");
        }
    }
}
