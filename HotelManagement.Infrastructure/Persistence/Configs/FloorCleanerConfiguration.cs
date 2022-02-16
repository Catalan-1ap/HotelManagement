using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelManagement.Infrastructure.Persistence.Configs
{
	public class FloorCleanerConfiguration : IEntityTypeConfiguration<FloorCleaner>
	{
		public void Configure(EntityTypeBuilder<FloorCleaner> builder)
		{
			builder
				.HasKey(x => new { x.CleanerId, x.FloorId });
		}
	}
}