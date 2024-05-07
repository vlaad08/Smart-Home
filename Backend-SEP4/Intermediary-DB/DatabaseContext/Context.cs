using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Logic;

public class Context : DbContext
{
    public DbSet<Home> Homes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Door> Doors { get; set; }
    public DbSet<HumidityReading> HumidityReadings { get; set; }
    public DbSet<LightReading> LightReadings { get; set; }
    public DbSet<TemperatureReading> TemperatureReadings { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Notification> Notifications { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ///cloud 
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SmartHome_db;Username=postgres;Password=password;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Home>().HasKey(h => h.Id);
        modelBuilder.Entity<Room>().HasKey(r => r.Id);
        modelBuilder.Entity<Door>().HasKey(d => d.Id);
        modelBuilder.Entity<HumidityReading>().HasKey(hr => hr.Id);
        modelBuilder.Entity<LightReading>().HasKey( lr=> lr.Id);
        modelBuilder.Entity<TemperatureReading>().HasKey(tr => tr.Id);
        modelBuilder.Entity<Admin>().HasKey(a => a.Id);
        modelBuilder.Entity<Member>().HasKey(m => m.Id);
        modelBuilder.Entity<Notification>().HasKey(n => n.Id);
        
    }
}