using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManagement.Infrastructure.Persistence.Configs
{
	public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
	{
		public static readonly int FirstNameMaxLength = 90;
		public static readonly int SurNameMaxLength = 90;
		public static readonly int PatronymicMaxLength = 90;


		public void Configure(EntityTypeBuilder<Person> builder)
		{
			builder
				.HasDiscriminator();

			builder
				.Property(p => p.FirstName)
				.HasMaxLength(FirstNameMaxLength);

			builder
				.Property(p => p.SurName)
				.HasMaxLength(SurNameMaxLength);

			builder
				.Property(p => p.Patronymic)
				.HasMaxLength(PatronymicMaxLength);
		}
	}
}