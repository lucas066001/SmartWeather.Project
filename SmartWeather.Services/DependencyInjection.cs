namespace SmartWeather.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Mqtt.Handlers;
using SmartWeather.Services.Stations;
using SmartWeather.Services.Users;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureServices();
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<StationService>();
        services.AddScoped<ComponentService>();
        services.AddScoped<MeasurePointService>();
        services.AddScoped<MeasureDataService>();
        services.AddScoped<ConfigRequestHandler>();
        services.AddScoped<SavingRequestHandler>();
        services.AddScoped<MqttService>();
        services.AddSingleton<MqttSingleton>();
    }
}
