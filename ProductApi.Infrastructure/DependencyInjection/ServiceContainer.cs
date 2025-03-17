using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config) 
        {
            // add database connectivity
            // add authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FineName"]!);

            // create dependency injection (DI)
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app) 
        {
            // register middleware such as :
            // global exception - handle external errors
            // listen to only api gateway - block all outside API calls
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
