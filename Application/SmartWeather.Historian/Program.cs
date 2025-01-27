using SmartWeather.Historian.Configuration;
using SmartWeather.Repositories;
using SmartWeather.Services;
using SmartWeather.Repositories.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDocumentRepositories(builder.Configuration);
builder.Services.AddMqttServices(builder.Configuration);
builder.Services.AddDocumentDbServices();
builder.Services.AddHostedService<MeasureDataConsumer>();

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
