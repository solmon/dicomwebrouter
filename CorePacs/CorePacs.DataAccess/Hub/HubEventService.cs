using CorePacs.Core.ApiInvoker;
using CorePacs.Core.ApiInvoker.Contracts;
using CorePacs.Core.ApiInvoker.Models;
using CorePacs.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Hub
{
    public class HubEventService : IHubEventService
    {
        private readonly IServiceInvoker _serviceInvoker;
        const string HUBURL = "HubEvents";
        public HubEventService(IServiceInvoker serviceInvoker) {
            if (serviceInvoker == null) throw new ArgumentNullException(nameof(serviceInvoker));
            this._serviceInvoker = serviceInvoker;            
        }
        public event EventHandler IncomingFile;
        public void NewIncomingFile(IncomingFileEventArgs evtArgs)
        {
            this._serviceInvoker.ConfigContext(HUBURL);
            var serviceMetadata = new ServiceMetaData() {

                Method = ServiceMethod.GET,
                PathSegments = new List<object>() {
                    "addnewfile"
                }
            };
            this._serviceInvoker.Invoke<string>(serviceMetadata).GetAwaiter().GetResult();
            //this.IncomingFile.Invoke(this, evtArgs);
        }
    }
}
