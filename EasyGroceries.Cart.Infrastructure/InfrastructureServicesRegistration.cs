using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Infrastructure.Contracts;
using EasyGroceries.Cart.Infrastructure.DBHandler;
using EasyGroceries.Cart.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ICartHeaderRepository, CartHeaderRepository>();
            services.AddScoped<ICartDetailsRepository, CartDetailsRepository>();
            services.AddScoped<IDapper, DatabaseHandler>();
            return services;
        }
    }
}
