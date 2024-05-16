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
    public DbSet<Member> member { get; set; }
    public DbSet<Notification> notification { get; set; }


    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   
        DotNetEnv.Env.TraversePath().Load();
        
        
        string SECRETSECTION_HOST = Environment.GetEnvironmentVariable("DATABASE_HOST");
        string SECRETSECTION_DB = Environment.GetEnvironmentVariable("DATABASE_NAME");
        string SECRETSECTION_USERNAME = Environment.GetEnvironmentVariable("DATABASE_USER");
        string SECRETSECTION_PASSWORD = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

        if (SECRETSECTION_HOST == null || SECRETSECTION_DB == null || SECRETSECTION_USERNAME == null || SECRETSECTION_PASSWORD == null)
        {
            DotNetEnv.Env.Load();
        }


        if(SECRETSECTION_HOST == null || SECRETSECTION_DB == null || SECRETSECTION_USERNAME == null || SECRETSECTION_PASSWORD == null)
        {
            throw new Exception("Environment variables not set");
        }
        
        optionsBuilder.UseNpgsql($"Host={SECRETSECTION_HOST};Port=5432;Database={SECRETSECTION_DB};Username={SECRETSECTION_USERNAME};Password={SECRETSECTION_PASSWORD};");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Home>().HasKey(h => h.Id);
        modelBuilder.Entity<Room>().HasKey(r => r.Id);
        modelBuilder.Entity<Door>().HasKey(d => d.Id);
        modelBuilder.Entity<HumidityReading>().HasKey(hr => hr.Id);
        modelBuilder.Entity<LightReading>().HasKey( lr=> lr.Id);
        modelBuilder.Entity<TemperatureReading>().HasKey(tr => tr.Id);
        modelBuilder.Entity<Member>().HasKey(m => m.Id);
        modelBuilder.Entity<Notification>().HasKey(n => n.Id);
        
    }
}