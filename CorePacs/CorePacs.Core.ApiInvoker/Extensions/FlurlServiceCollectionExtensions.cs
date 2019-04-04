using CorePacs.Core.ApiInvoker.Configuration;
using CorePacs.Core.ApiInvoker.Contracts;
using CorePacs.Core.ApiInvoker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePacs.Core.ApiInvoker.Extensions
{
    public static class FlurlServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpRestServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IServiceInvoker, ServiceInvoker>();
            services.AddOptions();
            services.Configure<ServicesConfig>(config.GetSection("ServicesConfig"));
            return services;
        }
    }
}
