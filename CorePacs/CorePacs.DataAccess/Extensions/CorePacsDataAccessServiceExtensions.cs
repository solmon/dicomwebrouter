using CorePacs.DataAccess.Config;
using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Hub;
using CorePacs.DataAccess.Repository;
using CorePacs.DataAccess.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Extensions
{
    public static class  CorePacsDataAccessServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<DStorageContext, DStorageContext>();
            services.AddTransient<IStorageRepository, StorageRepository>();

            services.AddTransient<IAETitleRepository, AETitlesRepository>();
            services.AddTransient<IDicomSendRepository, DicomSendRepository>();
            services.AddTransient<ILinkClientRepository, LinkClientRepository>();
            services.AddTransient<IRoutingTableRepository, RoutingTableRepository>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();

            services.AddSingleton<CorePacsSettings>(new CorePacsSettings());
            services.AddSingleton<IHubEventService, HubEventService>();
            services.AddTransient<IPathFinder, PathFinder>();
            services.AddTransient<IStorage, FileStorage>();
            return services;
        }

        public static void InitiailizeDataServices()
        {
            var context = ServiceLocator.ProviderInstance.GetRequiredService<DStorageContext>();
            context.Database.EnsureCreated();
            context.EnsureSeeded();            
        }
    }
}
