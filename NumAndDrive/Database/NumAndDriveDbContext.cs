using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NumAndDrive.Models;

namespace NumAndDrive.Database
{
	public class NumAndDriveDbContext : IdentityDbContext<User>
	{
		public NumAndDriveDbContext(DbContextOptions options) : base(options)
		{
		}

        public DbSet<ActivationDay> ActivationDays { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<ActivationDays_Journeys> ActivationDays_Journeys { get; set; }
        public DbSet<Addresses_Journeys> Addresses_Journeys { get; set; }
        public DbSet<Notifications_Users> Notifications_Users { get; set; }
        public DbSet<Filters_Journeys> Filters_Journeys { get; set; }
        public DbSet<Filters_Users> Filters_Users { get; set; }
        public DbSet<Journeys_Users> Journeys_Users { get; set; }
        public DbSet<Rewards_Users> Rewards_Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var connectionString = "server=localhost;port=3306;database=numanddrive;user=root;password=root;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
