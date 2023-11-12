using CourseWorkAdmin.Models;
using CourseWorkAdmins.Models;
using Microsoft.EntityFrameworkCore;


namespace CourseWorkAdmins.Data
{
    public class ComputerClubDBContext : DbContext
    {
        public DbSet<Office> Offices { get; set; } = null!;
        public DbSet<Administrator> Administrators { get; set; } = null!;
        public DbSet<Device> Devices { get; set; } = null!;
        public DbSet<Log> Logs { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<DeviceGame> DevicesGames { get; set; } = null!;
        public DbSet<Promo> Promos { get; set; } = null!;
        public DbSet<Rent> Rents { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceGame>().HasKey(dg => new { dg.DeviceId, dg.GameName });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-B5U2OSQ;Initial Catalog=ComputerClubDB;Integrated Security=true;encrypt=false;");
        }

    }
}
