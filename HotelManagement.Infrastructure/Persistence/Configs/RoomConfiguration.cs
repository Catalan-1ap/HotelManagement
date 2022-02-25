using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManagement.Infrastructure.Persistence.Configs
{
	public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
	{
		public static readonly int NumberMaxLength = 10;


		public void Configure(EntityTypeBuilder<Room> builder)
		{
			builder
				.Property(r => r.Number)
				.HasMaxLength(NumberMaxLength);

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
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}