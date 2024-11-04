namespace SmartWeather.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;
using SmartWeather.Repositories.Users;
using SmartWeather.Services.Authentication;
using SmartWeather.Repositories.Authentication;
using SmartWeather.Services.Repositories;
using SmartWeather.Services.Stations;
using SmartWeather.Repositories.Stations;
using SmartWeather.Services.Components;
using SmartWeather.Repositories.Components;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Repositories.ComponentDatas;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Repositories.MeasurePoints;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureRepositories();
    }

    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Setup database according to appsettings
        services.AddDbContext<SmartWeatherContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("SmartWeatherMaster") ?? string.Empty));
        services.AddDbContext<SmartWeatherReadOnlyContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("SmartWeatherLb") ?? string.Empty));
        services.AddScoped(typeof(SmartWeatherDocumentsContext));
    }

    private static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddScoped(typeof(IAuthenticationRepository), typeof(AuthenticationRepository));
        services.AddScoped(typeof(IStationRepository), typeof(StationRepository));
        services.AddScoped(typeof(IComponentRepository), typeof(ComponentRepository));
        services.AddScoped(typeof(IMeasurePointRepository), typeof(MeasurePointRepository));
        services.AddScoped(typeof(IMeasureDataRepository), typeof(MeasureDataRepository));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
