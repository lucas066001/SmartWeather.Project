using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;
using SmartWeather.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWeather.Services.Authentication;
using SmartWeather.Repositories.Authentication;
using SmartWeather.Services.Repositories;
using SmartWeather.Services.Stations;
using SmartWeather.Repositories.Stations;

namespace SmartWeather.Repositories
{
    public static class DependencyInjection
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDbContext(configuration);
            services.ConfigureRepositories();
        }

        private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Setup database according to appsettings
            services.AddDbContext<SmartWeatherContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SmartWeatherDatabase")));
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            // Setup interface implementation for upper dependencies

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IAuthenticationRepository), typeof(AuthenticationRepository));
            services.AddScoped(typeof(IStationRepository), typeof(StationRepository));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
