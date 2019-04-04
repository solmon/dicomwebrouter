using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Storage;
using CorePacs.Dicom.Config;
using CorePacs.Dicom.Contracts;
using CorePacs.Dicom.Server;
using CorePacs.Dicom.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Extensions
{    
    public static class CorePacsDicomServiceExtensions
    {
        public static IServiceCollection AddDicomServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IDicomParser, DicomParser>();
            services.AddTransient<IPacsServer, PacsServer>();
            services.AddTransient<IDicomEncrypter, DicomEncrypter>();
            services.AddTransient<IEncrypter, AesEncrypter>();
            services.AddTransient<IImageEncryptionService, ImageEncryption>();
            services.AddTransient<IEncryptionServer, EncryptionServer>();
            services.AddTransient<ILinkSendServer, LinkSendServer>();
            services.AddTransient<ILinkSendService, LinkSendService>();
            services.AddTransient<IDecryptionServer, DecryptionServer>();
            services.AddTransient<IDecryptionService, DecryptionService>();
            services.AddTransient<IDicomSendServer, DicomSendServer>();
            services.AddTransient<IDicomSendService, DicomSendService>();
            services.AddTransient<IDicomClient, DicomClientImpl>();
            services.AddSingleton<IRouteFinder, RouteFinder>();

            services.AddOptions();
            services.Configure<LinkKey>(config.GetSection("LinkKey"));

            return services;
        }
    }
}
