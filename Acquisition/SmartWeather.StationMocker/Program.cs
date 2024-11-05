using SmartWeather.Services;
using SmartWeather.Repositories;
using SmartWeather.StationMocker.Services;

Thread.Sleep(5000);
Thread.Sleep(5000);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHostedService<MockerHosted>();


var app = builder.Build();

app.Run();
