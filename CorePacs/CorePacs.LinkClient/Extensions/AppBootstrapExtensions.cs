using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.LinkClient.Extensions
{
    public static class AppBootstrapExtensions
    {
        public static IServiceCollection AddBootStrapServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<App>();
            return services;
        }
    }
}
