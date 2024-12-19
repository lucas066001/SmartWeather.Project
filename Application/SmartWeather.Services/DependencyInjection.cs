﻿namespace SmartWeather.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.Kafka;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Stations;
using SmartWeather.Services.Users;

public static class DependencyInjection
{
    public static void AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<KafkaConsumerSingleton>();
    }

    public static void AddMqttServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MqttService>();
        services.AddSingleton<MqttSingleton>();
    }

    public static void AddRelationalDbServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<StationService>();
        services.AddScoped<ComponentService>();
        services.AddScoped<MeasurePointService>();
    }

    public static void AddDocumentDbServices(this IServiceCollection services)
    {
        services.AddScoped<MeasureDataService>();
    }
}
