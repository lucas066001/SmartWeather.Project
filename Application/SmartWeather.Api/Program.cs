using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using SmartWeather.Services;
using SmartWeather.Repositories;
using SmartWeather.Services.Options;
using SmartWeather.Api.Configuration;
using SmartWeather.Repositories.Context;
using SmartWeather.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRelationalRepositories(builder.Configuration);
builder.Services.AddDocumentRepositories(builder.Configuration);
builder.Services.AddMqttServices(builder.Configuration);
builder.Services.AddRelationalDbServices();
builder.Services.AddDocumentDbServices();

builder.Services.AddScoped<AccessManagerHelper>();

var issuer = builder.Configuration.GetSection(nameof(Jwt))[nameof(Jwt.Issuer)];
var audience = builder.Configuration.GetSection(nameof(Jwt))[nameof(Jwt.Audience)];
var key = builder.Configuration.GetSection(nameof(Jwt))[nameof(Jwt.Key)];

if (string.IsNullOrEmpty(issuer) ||
    string.IsNullOrEmpty(audience) ||
    string.IsNullOrEmpty(key))
{
    throw new Exception("Unable to retreive structural token infos");
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()                   
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Services.AddControllers();
builder.Services.AddHostedService<PostStartup>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authorization using Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SmartWeatherContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
