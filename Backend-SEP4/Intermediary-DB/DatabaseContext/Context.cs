using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Logic;

public class Context : DbContext
{
    public DbSet<Home> home { get; set; }
    public DbSet<Room> room { get; set; }
    public DbSet<Door> door { get; set; }
    public DbSet<HumidityReading> humidity_reading { get; set; }
    public DbSet<LightReading> light_reading { get; set; }
    public DbSet<TemperatureReading> temperature_reading { get; set; }
    public DbSet<Admin> admin { get; set; }
    public DbSet<Member> member { get; set; }
    public DbSet<Notification> notification { get; set; }


    private string SECRETSECTION_HOST = "smart-homel.postgres.database.azure.com";
    private string SECRETSECTION_DB = "smart_home";
    private string SECRETSECTION_NAME = "sep_user";
    private string SECRETSECTION_PASSWORD = "Semester4Password";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ///cloud 
        optionsBuilder.UseNpgsql($"Host={SECRETSECTION_HOST};Port=5432;Database={SECRETSECTION_DB};Username={SECRETSECTION_NAME};Password={SECRETSECTION_PASSWORD};");
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