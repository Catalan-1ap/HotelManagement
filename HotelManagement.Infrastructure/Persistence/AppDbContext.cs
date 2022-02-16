using System;
using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;


namespace HotelManagement.Infrastructure.Persistence
{
	public class AppDbContext : DbContext
	{
		internal static readonly string ConnectionsString = 
			@$"Server=(localdb)\mssqllocaldb;AttachDbFilename={Environment.CurrentDirectory}\Persistence\Database\HotelManagement.mdf;Trusted_Connection=True;";

		public DbSet<Room> Rooms { get; set; } = null!;
		public DbSet<Person> Persons { get; set; } = null!;
		public DbSet<Client> Clients { get; set; } = null!;
		public DbSet<Cleaner> Cleaners { get; set; } = null!;
		public DbSet<Floor> Floors { get; set; } = null!;


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseSqlServer(ConnectionsString);
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ConfigureSmartEnum();

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}
	}
}