using SmartWeather.Repositories;
using SmartWeather.Services;
using SmartWeather.Socket.Api.Configuration;
using SmartWeather.Socket.Api.Hubs.MeasurePoint;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddConsumer();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseDefaultFiles();
app.MapHubs();

app.Run();
