using SmartWeather.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartWeather.Services;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Services.Options;
using System.Configuration;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using SmartWeather.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHostedService<PostStartup>();

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

builder.Services.AddControllers();

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
