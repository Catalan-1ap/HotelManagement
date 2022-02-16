using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
	public class RoomConfiguration : IEntityTypeConfiguration<Room>
	{
		public void Configure(EntityTypeBuilder<Room> builder)
		{
			builder
				.Property(p => p.RoomType)
				.HasConversion(
					p => p.Value,
					p => RoomType.FromValue(p));
		}
	}
}