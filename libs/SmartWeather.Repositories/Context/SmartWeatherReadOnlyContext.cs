namespace SmartWeather.Repositories.Context;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Entities.ActivationPlan;
using SmartWeather.Repositories.Context.Configurations;

public class SmartWeatherReadOnlyContext(DbContextOptions<SmartWeatherReadOnlyContext> options) : DbContext(options)
{
    public DbSet<User> Users { get { return Set<User>(); } }
    public DbSet<Station> Stations { get { return Set<Station>(); } }
    public DbSet<Component> Components { get { return Set<Component>(); } }
    public DbSet<MeasurePoint> MeasurePoints { get { return Set<MeasurePoint>(); } }
    public DbSet<ActivationPlan> ActivationPlans { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new StationConfiguration().Configure(modelBuilder.Entity<Station>());
        new ComponentConfiguration().Configure(modelBuilder.Entity<Component>());
        new MeasurePointConfiguration().Configure(modelBuilder.Entity<MeasurePoint>());
        new ActivationPlanConfiguration().Configure(modelBuilder.Entity<ActivationPlan>());

        base.OnModelCreating(modelBuilder);
    }
}
