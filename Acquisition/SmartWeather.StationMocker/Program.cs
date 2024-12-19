using SmartWeather.Services;
using SmartWeather.StationMocker.Services;

Thread.Sleep(5000);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMqttServices(builder.Configuration);
builder.Services.AddHostedService<MockerHosted>();

var app = builder.Build();

app.Run();
