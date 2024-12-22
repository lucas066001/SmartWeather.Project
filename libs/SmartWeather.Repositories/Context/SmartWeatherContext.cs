namespace SmartWeather.Repositories.Context;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context.Configurations;

public class SmartWeatherContext(DbContextOptions<SmartWeatherContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<MeasurePoint> MeasurePoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new StationConfiguration().Configure(modelBuilder.Entity<Station>());
        new ComponentConfiguration().Configure(modelBuilder.Entity<Component>());
        new MeasurePointConfiguration().Configure(modelBuilder.Entity<MeasurePoint>());

        base.OnModelCreating(modelBuilder);
    }
}
