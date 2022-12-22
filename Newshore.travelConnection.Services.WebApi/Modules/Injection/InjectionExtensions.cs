using Newshore.travelConnection.Application.Interface;
using Newshore.travelConnection.Application.Main;
using Newshore.travelConnection.Domain.Core;
using Newshore.travelConnection.Domain.Interface;
using Newshore.travelConnection.Infrastructure.Data;
using Newshore.travelConnection.Infrastructure.Interface;
using Newshore.travelConnection.Infrastructure.Repository;
using Newshore.travelConnection.Transversal.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newshore.travelConnection.Transversal.Logging;

namespace Newshore.travelConnection.Services.WebApi.Modules.Injection
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            ///Users
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
           /* services.AddScoped<IUsersApplication, UsersApplication>();
            services.AddScoped<IUsersDomain, UsersDomain>();
            services.AddScoped<IUsersRepository, UsersRepository>();*/

            ///catalog
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IJourneyAplication, JourneyAplication>();
            services.AddScoped<IJourneyDomain, JourneyDomain>();
            services.AddScoped<IServiceNewshoreAirDomain, ServiceNewshoreAirDomain>();
            services.AddScoped<IJourneyRepository, JourneyRepository>();

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));


            return services;
        }
    }
}
