using EasyGroceries.Cart.Application.Contracts.Services;
using EasyGroceries.Cart.Application.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using EasyGroceries.Cart.Application.Validators;

namespace EasyGroceries.Cart.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Product", u => u.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"]));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddValidatorsFromAssemblyContaining<CartDtoValidator>();
            return services;
        }
    } 
}
