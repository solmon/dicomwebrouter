using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CorePacs.DataAccess.Config
{
    public static class ServiceLocator
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static ServiceProvider ProviderInstance { get; set; }
        public static IServiceCollection ServicesInstance { get; set; }

        public static T GetInstance<T>() {

            return ProviderInstance.GetService<T>();
        }
    }
}
