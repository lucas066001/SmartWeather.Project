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
using SmartWeather.Repositories.BaseRepository;

public static class DependencyInjection
{
    public static void AddRelationalRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRelationalDbContext(configuration);
        services.ConfigureRelationalRepositories();
    }
    public static void AddDocumentRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDocumentDbContext(configuration);
        services.ConfigureDocumentRepositories();
    }

    private static void ConfigureRelationalDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SmartWeatherContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("SmartWeatherMaster") ?? string.Empty));
        services.AddDbContext<SmartWeatherReadOnlyContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("SmartWeatherLb") ?? string.Empty));
    }

    private static void ConfigureRelationalRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddScoped(typeof(IAuthenticationRepository), typeof(AuthenticationRepository));
        services.AddScoped(typeof(IStationRepository), typeof(StationRepository));
        services.AddScoped(typeof(IComponentRepository), typeof(ComponentRepository));
        services.AddScoped(typeof(IMeasurePointRepository), typeof(MeasurePointRepository));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    private static void ConfigureDocumentDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(SmartWeatherDocumentsContext));
    }

    private static void ConfigureDocumentRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IMeasureDataRepository), typeof(MeasureDataRepository));
    }
}
