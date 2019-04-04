using CorePacs.Core.ApiInvoker.Extensions;
using CorePacs.DataAccess.Config;
using CorePacs.DataAccess.Extensions;
using CorePacs.Dicom.Extensions;
using CorePacs.Extensions;
using CorePacs.WebServer.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CorePacs
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("hosting.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            ServiceLocator.Configuration = Configuration;            
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("CorePacs.WebServer")));
            services.AddDicomServices(Configuration);
            services.AddDataServices(Configuration);
            services.AddBootStrapServices(Configuration);
            services.AddHttpRestServices(Configuration);
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddSignalR();

            var serviceProvider  = services.BuildServiceProvider();
            ServiceLocator.ProviderInstance = serviceProvider;
            ServiceLocator.ServicesInstance =services;
            CorePacsDataAccessServiceExtensions.InitiailizeDataServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("CorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<IncomingHub>("incoming");
            });

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
