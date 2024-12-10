using SmartWeather.Services;
using SmartWeather.Repositories;
using SmartWeather.Historian.Configuration;
using SmartWeather.Repositories.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHostedService<PostStartup>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SmartWeatherDocumentsContext>();
    if (context != null)
    {
        await context.ConfigureIndexesAsync();
    }
}

app.UseHttpsRedirection();

app.Run();
