using CorePacs.DataAccess.Config;
using CorePacs.DataAccess.Extensions;
using CorePacs.Dicom.Extensions;
using CorePacs.LinkClient.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace CorePacs.LinkClient
{
    class Program
    {
        static ManualResetEvent resetEvent = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            try
            {
                Console.Write("CORE - APP INSIDE");
                RunWebServer();                
                resetEvent.WaitOne();                
            }
            catch (Exception ex) {
                Console.Write(ex);
            }
            
        }
        static void RunWebServer() {
            try
            {
                var host = new WebHostBuilder()
                .UseKestrel()
                //.UseContentRoot(pathToContentRoot)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:5001")
                //.UseApplicationInsights()
                .Build();

                var app = ServiceLocator.ProviderInstance.GetService<App>().Run(resetEvent).ConfigureAwait(false);
                //host.;
                host.Run();
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }
    }
}
